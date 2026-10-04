# monsterCompendium

Aplicación .NET MAUI (parcial de la asignatura Programación Apps Móviles 2) que actúa como compendio de monstruos de Dungeons & Dragons.

La app consume la API pública https://www.dnd5eapi.co/ para obtener datos de monstruos y los presenta en una interfaz multiplataforma (Android, iOS, Mac Catalyst y Windows).

Características
- Listado y búsqueda de monstruos (consumo REST de la API).
- Página de detalle con estadísticas y descripciones.
- Favoritos con persistencia local en SQLite.
- Arquitectura MVVM con CommunityToolkit.Mvvm.

Tecnologías
- .NET 10 y .NET MAUI
- CommunityToolkit.Mvvm
- sqlite-net-pcl y SQLitePCLRaw.bundle_green
- API: https://www.dnd5eapi.co/

Requisitos previos
- Visual Studio 2026 con cargas de trabajo para .NET MAUI (Android/iOS/Windows/Mac) instaladas.
- .NET 10 SDK.
- Emulador o dispositivo para la plataforma de destino (si aplica).

Instalación y ejecución
1. Clonar el repositorio:
   git clone https://github.com/PesiKickMan/monsterCompendium.git
2. Abrir la solución `monsterCompendium.slnx` en Visual Studio 2026.
3. Restaurar paquetes NuGet (Visual Studio lo hace automáticamente).
4. Seleccionar proyecto de inicio y plataforma objetivo (Android/iOS/Windows/MacCatalyst).
5. Ejecutar (F5) en emulador/dispositivo o en plataforma de escritorio.

Configuración de la API
- La aplicación usa directamente la API pública https://www.dnd5eapi.co/. No requiere clave/API key.
- Para cambiar la URL base, editar el servicio HTTP en el proyecto (buscar clases de cliente/servicio de red).

Persistencia local
- SQLite guarda los monstruos marcados como favoritos. La base de datos se crea automáticamente en el directorio de la app.

Estructura principal del proyecto
- Views/    : páginas XAML (Listado, Detalle, Favoritos)
- ViewModels/: lógica MVVM
- Models/   : modelos de datos
- Services/ : cliente HTTP y repositorio local (SQLite)
- Resources/: imágenes, iconos y splash

Notas para la entrega del parcial
- Indicar la plataforma utilizada para la demostración (p. ej. Android emulador o Windows).
- Incluir captura o vídeo corto mostrando búsqueda, detalle y favoritos.

Autor
Proyecto desarrollado como parcial de la asignatura Programación Apps Móviles 2 por Matias Candia.
