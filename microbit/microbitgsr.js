bluetooth.onBluetoothConnected(function () {
    basic.showIcon(IconNames.Yes)
})
bluetooth.onBluetoothDisconnected(function () {
    basic.showIcon(IconNames.No)
})
bluetooth.onUartDataReceived(serial.delimiters(Delimiters.Hash), function () {
    received = bluetooth.uartReadUntil(serial.delimiters(Delimiters.Hash))
    if (received == "G") {
        bluetooth.uartWriteLine("" + pins.analogReadPin(AnalogPin.P0))
    }
})
let received = ""
bluetooth.startUartService()
// Induláskor még nincs kapcsolat
basic.showIcon(IconNames.No)

