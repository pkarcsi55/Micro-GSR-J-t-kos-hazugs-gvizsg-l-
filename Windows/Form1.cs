using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Windows.Devices.Bluetooth;
using Windows.Devices.Bluetooth.Advertisement;
using Windows.Devices.Bluetooth.GenericAttributeProfile;
using Windows.Storage.Streams;

namespace BLE_test
{
    public partial class Form1 : Form
    {
        // =====================================================
        // BLE
        // =====================================================

        private BluetoothLEAdvertisementWatcher? watcher;
        private BluetoothLEDevice? microbit;

        private GattCharacteristic? txCharacteristic;
        private GattCharacteristic? rxCharacteristic;

        // Nordic UART Service
        private readonly Guid UART_SERVICE =
            Guid.Parse("6E400001-B5A3-F393-E0A9-E50E24DCCA9E");

        // micro:bit -> PC
        private readonly Guid UART_TX =
            Guid.Parse("6E400002-B5A3-F393-E0A9-E50E24DCCA9E");

        // PC -> micro:bit
        private readonly Guid UART_RX =
            Guid.Parse("6E400003-B5A3-F393-E0A9-E50E24DCCA9E");

        private bool plotStopped = false;
        // =====================================================
        // MÉRÉSI ADATOK
        // =====================================================

        private readonly List<double> plotData = new();
        private readonly List<int> rawValues = new();

        private const int MaxPoints = 250;
        private const int BaselineSamples = 10;

        private int baseValue = 0;
        private bool baseValueSet = false;
        private ulong? selectedMicrobitAddress = null;


        // =====================================================
        // FORM
        // =====================================================

        public Form1()
        {
            InitializeComponent();

            Resize += Form1_Resize;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            Text = "micro:bit GSR";
            MinimumSize = new Size(800, 500);

            button1.Text = "Keresés";
            buttonBaseline.Text = "Nullázás";

            textBoxData.Text = "Nincs kapcsolat.";

            PositionControls();

            formsPlot1.Plot.Title(
                "Bőrellenállás változása");

            formsPlot1.Plot.XLabel(
                "Mérési pont");

            formsPlot1.Plot.YLabel(
                "Relatív változás");

            formsPlot1.Plot.Axes.SetLimits(
                0,
                MaxPoints,
                -100,
                100);

            formsPlot1.Refresh();
        }
        private void Form1_Resize(object sender, EventArgs e)
        {
            PositionControls();
        }
        // =====================================================
        // VEZÉRLŐK ELHELYEZÉSE
        // =====================================================
        private void PositionControls()
        {
            // TableLayoutPanel
            tableLayoutPanel1.Dock = DockStyle.Top;

            // ComboBox: függőlegesen középen,
            // vízszintesen kitölti a cellát
            comboMicrobit.Anchor =
                AnchorStyles.Left | AnchorStyles.Right;

            // Gombok: cellán belül középen
            button1.Anchor = AnchorStyles.None;
            buttonConnect.Anchor = AnchorStyles.None;
            buttonBaseline.Anchor = AnchorStyles.None;
            buttonStopPlot.Anchor = AnchorStyles.None;

            // P0 kijelzés
            labelPin0.Anchor = AnchorStyles.None;

            // Státuszmező: függőlegesen középen,
            // vízszintesen kitölti a rendelkezésre álló helyet
            textBoxData.Anchor =
                AnchorStyles.Left | AnchorStyles.Right;

            labelPin0.Text = "P0: ---";

            // Grafikon kitölti a maradék területet
            formsPlot1.Dock = DockStyle.Fill;

            // Margók
            comboMicrobit.Margin = new Padding(5);
            button1.Margin = new Padding(5);
            buttonConnect.Margin = new Padding(5);
            buttonBaseline.Margin = new Padding(5);
            buttonStopPlot.Margin = new Padding(5);
            labelPin0.Margin = new Padding(5);
            textBoxData.Margin = new Padding(5);
        }

        // =====================================================
        // KAPCSOLÓDÁS GOMB
        // =====================================================

        private void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            button1.Text = "Keresés...";

            textBoxData.Text = "micro:bit keresése...";

            watcher = new BluetoothLEAdvertisementWatcher();

            watcher.ScanningMode =
                BluetoothLEScanningMode.Active;

            watcher.Received += Watcher_Received;
            foundMicrobits.Clear();
            comboMicrobit.Items.Clear();

            watcher.Start();
        }
        // =====================================================
        // NULLÁZÁS
        // =====================================================
        private void buttonBaseline_Click(object sender, EventArgs e)
        {
            if (rawValues.Count == 0)
                return;

            baseValue =
                (int)rawValues.Average();

            baseValueSet = true;

            plotData.Clear();

            UpdatePlot();
        }

        private void buttonStopPlot_Click(object sender, EventArgs e)
        {
            plotStopped = !plotStopped;

            buttonStopPlot.Text =
                plotStopped ? "Start plot" : "Stop plot";
        }
        // =====================================================
        // MICRO:BIT MEGTALÁLVA
        // =====================================================
        private async void Watcher_Received(BluetoothLEAdvertisementWatcher sender, BluetoothLEAdvertisementReceivedEventArgs args)
        {
            string name = args.Advertisement.LocalName;

            // Csak a micro:bitek érdekelnek
            if (string.IsNullOrEmpty(name) ||
                !name.Contains("micro:bit", StringComparison.OrdinalIgnoreCase))
                return;


            // =====================================================
            // MICRO:BIT FELVÉTELE A LISTÁBA
            // =====================================================

            BeginInvoke(() =>
            {
                if (!foundMicrobits.ContainsKey(name))
                {
                    foundMicrobits[name] = args.BluetoothAddress;

                    comboMicrobit.Items.Add(name);

                    if (comboMicrobit.SelectedIndex < 0)
                        comboMicrobit.SelectedIndex = 0;
                }
            });


            // =====================================================
            // HA MÉG NINCS KIVÁLASZTOTT ESZKÖZ,
            // CSAK GYŰJTÜNK
            // =====================================================

            if (selectedMicrobitAddress == null)
                return;


            // Nem ezt a micro:bitet választottuk
            if (args.BluetoothAddress != selectedMicrobitAddress.Value)
                return;


            // =====================================================
            // A KIVÁLASZTOTT MICRO:BIT JELENTKEZETT
            // =====================================================

            // Több advertisement már ne indítson
            // párhuzamos kapcsolódást
            selectedMicrobitAddress = null;

            //watcher?.Stop();

            try
            {
                // =====================================================
                // BLE KAPCSOLAT
                // =====================================================

                microbit =
                    await BluetoothLEDevice.FromBluetoothAddressAsync(
                        args.BluetoothAddress);

                if (microbit == null)
                {
                    ConnectFailed("Nem sikerült kapcsolódni.");
                    return;
                }


                // =====================================================
                // UART SERVICE
                // =====================================================

                var serviceResult =
                    await microbit.GetGattServicesForUuidAsync(
                        UART_SERVICE,
                        BluetoothCacheMode.Uncached);

                if (serviceResult.Status != GattCommunicationStatus.Success)
                {
                    ConnectFailed(
                        "UART service hiba. Status: " +
                        serviceResult.Status);

                    return;
                }

                if (serviceResult.Services.Count == 0)
                {
                    ConnectFailed(
                        "UART service nincs a service listában.");

                    return;
                }

                GattDeviceService uartService =
                    serviceResult.Services[0];


                // =====================================================
                // TX: micro:bit -> PC
                // =====================================================

                var txResult =
                    await uartService.GetCharacteristicsForUuidAsync(
                        UART_TX,
                        BluetoothCacheMode.Uncached);

                if (txResult.Status != GattCommunicationStatus.Success ||
                    txResult.Characteristics.Count == 0)
                {
                    ConnectFailed("TX characteristic nem érhető el.");
                    return;
                }

                txCharacteristic =
                    txResult.Characteristics[0];

                txCharacteristic.ValueChanged +=
                    TxCharacteristic_ValueChanged;


                // =====================================================
                // INDICATION
                // =====================================================

                var indicationStatus =
                    await txCharacteristic
                        .WriteClientCharacteristicConfigurationDescriptorAsync(
                            GattClientCharacteristicConfigurationDescriptorValue.Indicate);

                if (indicationStatus != GattCommunicationStatus.Success)
                {
                    ConnectFailed(
                        "Indication hiba: " + indicationStatus);

                    return;
                }


                // =====================================================
                // RX: PC -> micro:bit
                // =====================================================

                var rxResult =
                    await uartService.GetCharacteristicsForUuidAsync(
                        UART_RX,
                        BluetoothCacheMode.Uncached);

                if (rxResult.Status != GattCommunicationStatus.Success ||
                    rxResult.Characteristics.Count == 0)
                {
                    ConnectFailed("RX characteristic nem érhető el.");
                    return;
                }

                rxCharacteristic =
                    rxResult.Characteristics[0];


                // =====================================================
                // KAPCSOLAT KÉSZ
                // =====================================================

                BeginInvoke(() =>
                {
                    textBoxData.Text =
                        "Kapcsolódva: " + name;

                    buttonConnect.Text = "Kapcsolódva";

                    rawValues.Clear();
                    plotData.Clear();
                    baseValueSet = false;

                    UpdatePlot();

                    timerMeasure.Start();
                });
            }
            catch (Exception ex)
            {
                ConnectFailed(
                    "Kapcsolódási hiba: " + ex.Message);
            }
        }
        private readonly Dictionary<string, ulong> foundMicrobits = new();
        // =====================================================
        // MÉRÉSI PARANCS KÜLDÉSE
        // =====================================================
        private async Task SendMeasureCommand()
        {
            if (rxCharacteristic == null)
                return;

            try
            {
                using DataWriter writer =
                    new DataWriter();

                writer.WriteString("G#");

                await rxCharacteristic
                    .WriteValueAsync(
                        writer.DetachBuffer(),
                        GattWriteOption.WriteWithResponse);
            }
            catch (Exception ex)
            {
                timerMeasure.Stop();

                ShowStatus(
                    "Küldési hiba: " +
                    ex.Message);
            }
        }
        // =====================================================
        // TIMER - 100 ms
        // =====================================================
        private async void timerMeasure_Tick(object sender, EventArgs e)
        {
            await SendMeasureCommand();
        }
        // =====================================================
        // ADAT ÉRKEZETT A MICRO:BIT-TŐL
        // =====================================================
        private void TxCharacteristic_ValueChanged(GattCharacteristic sender,GattValueChangedEventArgs args)
        {
            try
            {
                DataReader reader =
                    DataReader.FromBuffer(
                        args.CharacteristicValue);

                byte[] data =
                    new byte[reader.UnconsumedBufferLength];

                reader.ReadBytes(data);

                string text =
                    Encoding.UTF8
                        .GetString(data)
                        .Trim();

                if (!int.TryParse(
                        text,
                        out int rawValue))
                    return;


                BeginInvoke(() =>
                {
                    // Aktuális P0 érték mindig frissül
                    labelPin0.Text = "P0: " + rawValue;


                    // Utolsó 10 nyers mérés tárolása
                    rawValues.Add(rawValue);

                    if (rawValues.Count >
                        BaselineSamples)
                    {
                        rawValues.RemoveAt(0);
                    }


                    // Első 10 mérés átlaga legyen
                    // az automatikus baseline
                    if (!baseValueSet)
                    {
                        if (rawValues.Count <
                            BaselineSamples)
                        {
                            return;
                        }

                        baseValue =
                            (int)rawValues.Average();

                        baseValueSet = true;
                    }


                    // A grafikon csak akkor frissül,
                    // ha nincs megállítva
                    if (!plotStopped)
                    {
                        // Relatív mérési érték
                        double value =
                            rawValue - baseValue;

                        plotData.Add(value);


                        // Csak az utolsó 250 pont
                        if (plotData.Count >
                            MaxPoints)
                        {
                            plotData.RemoveAt(0);
                        }


                        UpdatePlot();
                    }
                });
            }
            catch (Exception ex)
            {
                ShowStatus(
                    "Adatolvasási hiba: " +
                    ex.Message);
            }
        }
        // =====================================================
        // GRAFIKON
        // =====================================================
        private void UpdatePlot()
        {
            formsPlot1.Plot.Clear();

            if (plotData.Count > 1)
            {
                double[] values = plotData.ToArray();
                var signal = formsPlot1.Plot.Add.Signal(values);
                signal.LineWidth = 2;
            }

            formsPlot1.Refresh();
        }

        // =====================================================
        // STÁTUSZ
        // =====================================================

        private void ShowStatus(string text)
        {
            if (InvokeRequired)
            {
                BeginInvoke(() =>
                    ShowStatus(text));

                return;
            }

            textBoxData.Text = text;
        }
        private void buttonConnect_Click(object sender, EventArgs e)
        {
            if (comboMicrobit.SelectedItem == null)
            {
                textBoxData.Text = "Válassz egy micro:bitet!";
                return;
            }

            string name = comboMicrobit.SelectedItem.ToString()!;

            if (!foundMicrobits.TryGetValue(name, out ulong address))
            {
                textBoxData.Text =
                    "A kiválasztott micro:bit nem található.";
                return;
            }

            // Megjegyezzük, melyik micro:bithez akarunk kapcsolódni
            selectedMicrobitAddress = address;

            buttonConnect.Enabled = false;
            buttonConnect.Text = "Kapcsolódás...";

            textBoxData.Text =
                "Kapcsolódás: " + name;

            // Ha a kereső esetleg áll, újraindítjuk.
            // A tényleges kapcsolódást majd a Watcher_Received végzi.
            if (watcher != null &&
                watcher.Status != BluetoothLEAdvertisementWatcherStatus.Started)
            {
                watcher.Start();
            }
        }
        private void ConnectFailed(string message)
        {
            if (InvokeRequired)
            {
                BeginInvoke(() => ConnectFailed(message));
                return;
            }

            timerMeasure.Stop();

            textBoxData.Text = message;

            buttonConnect.Enabled = true;
            buttonConnect.Text = "Kapcsolódás";
        }

        
    }
}