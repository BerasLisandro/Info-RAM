# Info-RAM — Información técnica de RAM y placa madre para Windows 🖥️🔍

[![Releases](https://img.shields.io/badge/Releases-descargar-blue?logo=github)](https://github.com/BerasLisandro/Info-RAM/releases)

Descripción corta: Aplicación de escritorio en C# (.NET 8, Windows Forms) para mostrar datos técnicos de la memoria RAM y la placa madre. Dirigida a técnicos y usuarios curiosos que quieran ver la configuración interna del equipo.

Índice
- Características
- Capturas
- Requisitos
- Descargar y ejecutar
- Guía rápida
- Información técnica
- Arquitectura y dependencias
- Desarrollo y compilación
- Pruebas
- Buenas prácticas
- Contribuir
- Licencia
- Contacto

Características
- Detecta módulos de RAM: tipo (DDR3/DDR4/DDR5), frecuencia nominal, velocidad real, tamaño por módulo, número de bancos.
- Lee información de la placa madre: fabricante, modelo, versión de BIOS/UEFI, chipset.
- Muestra identificadores y datos S.M.A.R.T. disponibles y otros marcadores de integridad.
- Exporta reporte a CSV y JSON para archivado o análisis.
- Interfaz clara con fichas (tabs) para RAM, placa madre y exportación.
- Funciona en Windows 10 y Windows 11 con .NET 8 instalado.
- Log y modo diagnóstico para técnicos.

Capturas
- Vista principal (RAM):  
  ![RAM view](https://images.unsplash.com/photo-1518770660439-4636190af475?q=80&w=1200&auto=format&fit=crop&ixlib=rb-4.0.3&s=8f5f9f6f6f6f6f6f6f6f6f6f6f6f6f6f)
- Vista placa madre:  
  ![Motherboard](https://images.unsplash.com/photo-1581091012184-7c6f9ce9d6a3?q=80&w=1200&auto=format&fit=crop&ixlib=rb-4.0.3&s=6a7c4d9a3b1a4b5c9a7d8e2f6c7b8a9d)

Requisitos
- Windows 10 o superior.
- .NET 8 Desktop Runtime (x86/x64 según build).
- Derechos de usuario necesarios para leer la información de hardware (la app intenta no requerir elevación, pero algunas lecturas pueden necesitar permisos).
- CPU x86_64 (la build principal es para sistemas Windows de 64 bits).

Descargar y ejecutar
- Visita las Releases y descarga el instalador o el ejecutable desde: https://github.com/BerasLisandro/Info-RAM/releases  
  Debe descargar el archivo correspondiente y ejecutarlo en la máquina de destino.  
- Si la página de Releases no está disponible, revisa la pestaña "Releases" del repositorio en GitHub.

Instalación (pasos)
1. Descargue el archivo .zip o el instalador (.msi/.exe) desde las Releases.
2. Extraiga el .zip o ejecute el instalador.
3. Abra Info-RAM.exe desde el menú Inicio o la carpeta de instalación.
4. Revise los logs si necesita detalle técnico.

Guía rápida
- Abrir la app: la ventana principal muestra un resumen de RAM y placa madre.
- Pestaña RAM: lista de módulos instalados, velocidad actual, tamaño y latencias.
- Pestaña Motherboard: fabricante, modelo, BIOS/UEFI, versión de chipset.
- Exportar: use el botón Exportar para guardar un CSV o JSON con toda la información.
- Diagnóstico: active el modo diagnóstico para generar un log detallado y un volcado de datos.

Ejemplo de salida (CSV)
- timestamp,module_index,size_mb,type,speed_mhz,manufacturer,part_number,serial
- 2025-08-01T12:34:56,1,8192,DDR4,3200,Kingston,9905678,ABCD1234

Información técnica (qué lee y cómo)
- Se usan APIs de Windows Management Instrumentation (WMI) y llamadas nativas donde WMI no reporta datos.
- Clases relevantes en código:
  - ManagementObjectSearcher para Win32_PhysicalMemory y Win32_BaseBoard.
  - P/Invoke para llamadas a SetupAPI y lectura de identificadores del PCB cuando es necesario.
- Datos leídos de RAM:
  - Banco físico, tamaño, tipo DDR, velocidad etiquetada y velocidad medida, manufacturer y número de pieza.
  - JEDEC ID cuando está disponible.
- Datos leídos de placa madre:
  - Fabricante, producto, versión, número de serie (si expone), versión de BIOS/UEFI, información del chipset y puertos disponibles.
- Exportador: usa System.Text.Json para JSON y System.IO para CSV.

Arquitectura y dependencias
- Lenguaje: C# 12 (.NET 8).
- UI: Windows Forms.
- Paquetes NuGet:
  - System.Management (WMI)
  - Newtonsoft.Json (opcional para compatibilidad)
  - CsvHelper (opcional para exportar CSV)
- Estructura:
  - /src: código fuente
  - /src/UI: formularios y recursos visuales
  - /src/Core: lógica de detección y exportación
  - /logs: salida de diagnóstico en ejecución local

Desarrollo y compilación
- Requisitos para compilación:
  - .NET 8 SDK
  - Visual Studio 2022/2023 o dotnet CLI
- Compilar:
  - dotnet build -c Release
  - Ejecutable en bin/Release/net8.0/win-x64 (según runtime)
- Empaquetado:
  - dotnet publish -c Release -r win-x64 --self-contained false -p:PublishSingleFile=true
- Tests unitarios:
  - dotnet test en el proyecto de pruebas para validar parsers y exportadores.

Pruebas y validación
- Tests incluidos:
  - Lectura de entradas simuladas (mock) para WMI.
  - Validación de CSV/JSON generado.
  - Tests de UI automatizados para botones clave (exportar, refrescar).
- Recomendación: correr pruebas en una VM antes de desplegar en equipo de producción.

Buenas prácticas al usar Info-RAM
- Cerrar otras herramientas que puedan bloquear acceso a hardware antes de ejecutar lecturas avanzadas.
- Exportar los reportes y conservar el CSV/JSON para tareas de soporte remoto.
- Mantener el .NET Desktop Runtime actualizado para compatibilidad.

Casos de uso
- Inventario de hardware: inventario rápido de módulos de RAM por equipo.
- Diagnóstico: confirmar que la velocidad y la cantidad coinciden con lo esperado.
- Reemplazo de módulos: identificar número de parte y fabricante antes de comprar.
- Soporte remoto: generar un JSON para enviar al equipo de soporte.

Seguridad y privacidad (comportamiento)
- La app no sube datos a servidores externos. Exporta archivos locales.
- Los logs pueden contener identificadores del hardware; guárelos de forma segura.
- Para compartir un reporte, revise y elimine seriales si necesita anonimizar.

Contribuir
- Cómo contribuir:
  1. Fork del repositorio.
  2. Crear una rama feature/tema.
  3. Abrir Pull Request con descripción clara del cambio.
- Buscamos:
  - Mejoras en detección de nuevas DDR (p. ej. DDR5).
  - Soporte para lecturas desde chipsets nuevos.
  - Traducciones y ajustes de UI.
- Estilo de código:
  - C# moderno, nullables activos, async donde corresponda.
  - Comentarios XML para métodos públicos.
  - Tests unitarios para lógica no UI.

Roadmap
- Próximas mejoras:
  - Integrar detección de XMP/overclock y perfiles de memoria.
  - Soporte para lectura directa de SPD cuando esté disponible.
  - Mejoras de rendimiento en escaneo simultáneo.
  - Builds firmados y MSI oficial.

Problemas comunes y soluciones
- No detecta algunos módulos:
  - Verifique permisos de lectura y que no hay software bloqueando acceso. Reinicie la app.
- Velocidad mostrada menor a la esperada:
  - Algunas placas muestran la velocidad base. Compare contra el BIOS/UEFI.
- Exporte JSON y abra con un viewer si el CSV no muestra todos los campos.

Etiquetas del repositorio
- csharp, desktop-app, diagnostic-tool, dotnet, hardware-info, motherboard-info, ram-info, system-information, windows, windows-forms

Licencia
- Repositorio bajo MIT License. Consulte el archivo LICENSE en el repositorio para términos completos.

Releases y descargas
- Descargue el instalador o ejecutable desde la página de Releases en GitHub: https://github.com/BerasLisandro/Info-RAM/releases  
  Descargue el archivo que corresponde a su sistema y ejecútelo para instalar o ejecutar la aplicación.

Contacto
- Para reportar bugs y solicitar funciones: abra un Issue en GitHub.
- Para contribuciones mayores: abra un PR con descripción del cambio y pruebas.

Créditos
- Desarrollador: BerasLisandro (repositorio principal).
- Basado en APIs de Windows y bibliotecas open source para exportación y parsing.

Registro de cambios (ejemplo)
- v1.0.0
  - Lanzamiento inicial con detección de RAM y placa madre.
  - Exportador CSV/JSON.
  - Interfaz Windows Forms.

Atajos útiles
- Ctrl+R: refrescar lecturas.
- Ctrl+E: exportar reporte.
- F1: abrir ayuda integrada.

Logs y diagnóstico
- La aplicación genera logs en la carpeta de usuario bajo %AppData%/Info-RAM/logs.
- Incluye timestamp y tipo de lectura.
- Use logs para comparar resultados entre sistemas.

Integración en flujos técnicos
- Use export CSV para alimentar inventarios automáticos.
- Integre JSON con scripts de despliegue para validar hardware en imagenes de referencia.

Mantenimiento
- Actualice la app desde Releases cuando haya nueva versión.
- Verifique compatibilidad de .NET Runtime cuando actualice Windows.

Agradecimientos
- Gracias a la comunidad open source por bibliotecas de parsing y exportación.
- Imágenes de referencia de Unsplash y recursos libres.

Reportar un problema
- Abra un Issue en el repositorio con:
  - Descripción del problema.
  - Log adjunto.
  - Versión de Info-RAM y Windows.
  - Pasos para reproducir.

Releases: [Ir a Releases](https://github.com/BerasLisandro/Info-RAM/releases)