# AsistenteIA – Asistente académico móvil con IA

Proyecto completo .NET MAUI (MVVM, SQLite, API REST de IA). Programación Avanzada y Aplicada – VI semestre.

## Ejecutar
1. Descomprima el .zip y abra **AsistenteIA.sln** con Visual Studio (carga de trabajo "Desarrollo de la interfaz de usuario de aplicaciones multiplataforma de .NET").
2. Espere la restauración de paquetes NuGet.
3. Seleccione **Windows Machine** (o un emulador Android) y pulse ▶.
4. En la pestaña **Configuración** pegue su clave de https://gen.pollinations.ai, pulse *Guardar clave* y *Probar conexión*.

Si su Visual Studio usa .NET 9, cambie `net10.0` por `net9.0` en `AsistenteIA.csproj`.

## Seguridad
No hay claves en el código ni en el repositorio: la clave se ingresa en la app y se guarda con SecureStorage.
