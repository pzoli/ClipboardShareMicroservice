# Clipboard share DotNet microservice

This is a clipboard share service made with Visual Studio 2019 and DotNet SDK 5.0.

## Build in command line

Enter root directory and use command:

dotnet build

## Run

cd bin\Debug\net5.0

ClipboardShareMicroservice.exe --urls http://192.168.1.139:5000

where you must change the IP address.

## Clipboard QRCodes

- http://192.168.1.139:5000/clipboard/qrcode/get (QRCode for get clipboard content from a PC to an Android device)
- http://192.168.1.139:5000/clipboard/qrcode/post (QRCode for post clipboard content from an Android device to PC)

You may want to put images to your desktop for better performance of usage.