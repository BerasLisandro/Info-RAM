# Info-RAM: Herramienta de Diagnóstico de Hardware

**Info-RAM** es una aplicación de escritorio desarrollada en C# (.NET 8, Windows Forms), diseñada para mostrar información técnica detallada sobre la memoria RAM y la placa madre (motherboard) de una PC. Está orientada tanto a técnicos de computadoras como a usuarios curiosos que deseen conocer la configuración interna de su equipo de forma rápida y sencilla.

## ✨ Características Principales

*   **Análisis Exhaustivo de Memoria RAM:**
    *   Identificación de fabricante, tipo (DDR, DDR2, DDR3, DDR4, DDR5, etc.), formato (DIMM, SO-DIMM).
    *   Detalles sobre capacidad total, velocidad, uso actual y slots disponibles.
*   **Información Detallada de la Placa Base:**
    *   Muestra el modelo y fabricante de la motherboard.
*   **Interfaz de Usuario Intuitiva y Moderna:**
    *   Diseño limpio con paneles estilizados y una paleta de colores profesional.
    *   Integración de iconos e imágenes de alta calidad directamente en el ejecutable.
    *   Botones interactivos con diseño atractivo para una experiencia de usuario mejorada.
*   **Funcionalidades Clave:**
    *   **Actualización en Tiempo Real:** Refresca la información del hardware con un solo clic.
    *   **Generación de Reportes:** Exporta un informe técnico completo a un archivo de texto (`Reporte_RAM.txt`), incluyendo un banner ASCII personalizado.
    *   **Información del Desarrollador:** Acceso rápido a los datos del autor de la aplicación.

## 📷 Capturas de pantalla

<p align="center">
  <img src="images/preview.png" alt="Vista previa de la aplicación" width="600"/>
</p>

Pantalla principal de InfoRAM mostrando datos de la memoria RAM y motherboard.

## 🖥️ Requisitos del Sistema

*   **Sistema Operativo:** Windows 8, Windows 10 o Windows 11 (64-bit).
*   **Framework:** .NET 8.0 Runtime.
*   **Arquitectura**: x64 

## 🚀 Instalación y Uso

### Ejecutable Precompilado

1.  Descarga la última versión desde la sección `Releases` del repositorio de GitHub.
2.  Descomprime el archivo `.zip` o `.rar` descargado.
3.  Ejecuta `InfoRAM.exe`.

### Compilación desde el Código Fuente

1.  Clona el repositorio:
    ```powershell
    git clone https://github.com/Pablitus666/Info-RAM.git
    ```
2.  Abre la solución `InfoRAMApp.sln` con Visual Studio 2022 o una versión posterior.
3.  Compila el proyecto en configuración `Release`.
4.  El ejecutable se encontrará en el directorio `bin/Release/net8.0-windows/`.

## 📝 Guardar Reporte

La aplicación permite guardar un reporte técnico detallado de la configuración de RAM y placa base de tu sistema. Este reporte se guarda como un archivo `Reporte_RAM.txt` e incluye un banner ASCII distintivo al inicio.

Ejemplo del banner incluido en el reporte:

```
_ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _
	 ____       _     _ _ _
	|  _ \ __ _| |__ | (_) |_ _   _ ___
	| |_) / _  | '_ \| | | __| | | / __|
	|  __/ (_| | |_) | | | |_| |_| \__ \_ _ _
	|_|   \__,_|_.__/|_|_|\__|\\__,_|___(_|_|_)
_ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _
```

## 🔒 Licencia

Este proyecto es de naturaleza **propietaria**. Queda estrictamente prohibido su uso, distribución o modificación sin la autorización expresa del autor.

## 👨‍💻 Autor

*   **Nombre:** Pablo Téllez
*   **Contacto:** pharmakoz@gmail.com