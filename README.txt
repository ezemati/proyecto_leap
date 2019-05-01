Pequeño tutorial para que funcione la biblioteca de Leap:

1.Click derecho - propiedades en el proyecto OtraPrueba2 adentro de la solución
2.Ir a la seccion de "Eventos de compilación"
3.En donde dice "Línea de comandos del evento posterior a la compilación:", escribir el siguiente comando:

xcopy /yr "[RUTA DEL PROYECTO]\LeapSDK\lib\x64\LeapC.dll" "$(TargetDir)"

Cambiando [RUTA DEL PROYECTO] por la ruta donde se encuentre este archivo de README.txt
4.Guardar
5.Click derecho en referencias, agregar referencia
6.Tocar examinar y buscar adentro de la carpeta de LeapSDK\lib, el archvio LeapCSharp.NET3.5.dll
7.Seleccionar (asegurandose de que aparezca el tick) y tocar aceptar 

Y listo :v