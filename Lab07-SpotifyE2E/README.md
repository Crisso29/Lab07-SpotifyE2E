<div align="center">

# UNIVERSIDAD NACIONAL DE SAN CRISTÓBAL DE HUAMANGA
### Escuela Profesional de Ingeniería de Sistemas

<br>

# 📄 INFORME TÉCNICO: AUTOMATIZACIÓN E2E
## Laboratorio 07: Pruebas y Aseguramiento de Calidad en Spotify Web Player

<br>

![.NET](https://img.shields.io/badge/.NET_10.0-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=c-sharp&logoColor=white)
![Playwright](https://img.shields.io/badge/Playwright-2EAD33?style=for-the-badge&logo=playwright&logoColor=white)
![NUnit](https://img.shields.io/badge/NUnit-0054A6?style=for-the-badge&logo=nunit&logoColor=white)
![Tests](https://img.shields.io/badge/TESTS-10%20PASS%20/%202%20FAIL-brightgreen?style=for-the-badge)
![Estado](https://img.shields.io/badge/ESTADO-COMPLETADO-blue?style=for-the-badge)

<br><br>

**ASIGNATURA:** IS-489 Pruebas y Aseguramiento de Calidad de Software <br>
**DOCENTE:** Ing. Lizbeth Jaico Quispe <br>
**ESTUDIANTE:** Crisólogo Aguilar Flores <br>
**LUGAR Y FECHA:** Ayacucho, Perú - Junio 2026

<br>

*(Documentación generada en cumplimiento de los estándares de Ingeniería de Calidad)*

---

</div>

## 📑 Índice
1. [Resumen Ejecutivo](#1-resumen-ejecutivo)
2. [Arquitectura y Stack Tecnológico](#2-arquitectura-y-stack-tecnológico)
3. [Estrategia de Autenticación (Anti-Bot Bypass)](#3-estrategia-de-autenticación-anti-bot-bypass)
4. [Matriz de Pruebas E2E (Playwright)](#4-matriz-de-pruebas-e2e-playwright)
5. [Ejecución y Generación de Reportes](#5-ejecución-y-generación-de-reportes)
6. [Hallazgos y Defectos Identificados](#6-hallazgos-y-defectos-identificados)
7. [Conclusiones](#7-conclusiones)

---

## 1. Resumen Ejecutivo
El presente repositorio documenta el diseño, implementación y ejecución de una suite de pruebas End-to-End (E2E) sobre la plataforma interactiva **Spotify Web Player**. Este proyecto eleva las exigencias del Laboratorio 07 migrando del entorno base sugerido en JavaScript/TypeScript hacia una arquitectura de grado empresarial utilizando **C# y .NET**.

Se aplicaron técnicas formales de testing de caja negra (Partición de Equivalencias, Análisis de Valor Límite y Casos Extremos/Seguridad) para evaluar 12 casos de prueba distribuidos en los módulos de **Búsqueda** y **Gestión de Playlists**. El proyecto demuestra el uso avanzado del patrón **Page Object Model (POM)** y la persistencia de sesiones complejas.

---

## 2. Arquitectura y Stack Tecnológico
Para garantizar la escalabilidad y mantenibilidad del código de automatización, el repositorio se divide en dos proyectos independientes, aplicando el principio de separación de responsabilidades:

* **`SpotifyE2E.Tests`:** Suite principal de pruebas NUnit. Implementa el patrón POM a través de la carpeta `Pages/` (`HomePage.cs`, `PlaylistPage.cs`, `SearchPage.cs`), aislando los selectores web (locators) de la lógica de aserción.
* **`LoginTool`:** Proyecto de consola auxiliar diseñado exclusivamente para gestionar la autenticación inicial y capturar los tokens de sesión.

---

## 3. Estrategia de Autenticación (Anti-Bot Bypass)
Uno de los mayores retos de la automatización E2E en plataformas modernas es la evasión de sistemas anti-bot (CAPTCHAs, bloqueos por velocidad de escritura). Para solucionar esto sin comprometer credenciales en el código fuente, se implementó el mecanismo **`storageState`**:

1.  El usuario ejecuta `LoginTool` localmente. Se levanta una instancia de Chromium inyectando el flag `--disable-blink-features=AutomationControlled` para ocultar la huella de Playwright.
2.  El usuario inicia sesión manualmente en la interfaz gráfica.
3.  La herramienta captura el contexto (Cookies, LocalStorage) y genera el archivo `spotify_state.json`.
4.  La suite de pruebas `SpotifyE2E.Tests` inyecta este estado globalmente a través del archivo `AuthSetup.cs`, permitiendo que todos los tests se ejecuten de forma autenticada instantáneamente y en paralelo.

---

## 4. Matriz de Pruebas E2E (Playwright)

Los siguientes casos fueron ejecutados simulando interacciones humanas reales (clics, tipado en teclado, navegación) sobre el DOM renderizado del navegador.

### 4.1. Módulo de Gestión de Playlists
Evalúa los flujos críticos de creación de colecciones, límites de caracteres y aserciones visuales de la interfaz.

| ID | Descripción del Caso | Técnica de Testing | Resultado Esperado | Estado |
|:---|:---|:---:|:---|:---:|
| **TC-001** | Creación exitosa de playlist | PE-Válida | Playlist creada. URL actualiza a `/playlist/...`. | ❌ FAIL* |
| **TC-002** | Edición de nombre y descripción | PE-Válida | Interfaz refleja actualización inmediata tras guardar. | ✅ PASS |
| **TC-003** | Descripción > 300 caracteres | PE-Inválida | Sistema trunca la entrada visual a 300 chars. | ✅ PASS |
| **TC-004** | Nombre límite exacto: 100 caracteres | AVL (N) | Spotify permite y renderiza nombre completo. | ✅ PASS |
| **TC-005** | Nombre límite excedido: 101 caracteres | AVL (N+1) | Input bloquea el tipado del carácter 101. | ✅ PASS |
| **TC-006** | Intento de guardar nombre vacío | Edge Case | Botón "Guardar" se deshabilita o muestra error. | ✅ PASS |

*(Nota: El TC-001 experimentó un `TimeoutException` de 58s derivado de la latencia de carga en la aserción del botón "Crear", evidenciando la volatilidad de los tiempos de respuesta en entornos web dinámicos).*

### 4.2. Módulo de Búsqueda
Validación de tolerancia a fallos, soporte de caracteres y protección contra ataques básicos en el frontend.

| ID | Descripción del Caso | Técnica de Testing | Resultado Esperado | Estado |
|:---|:---|:---:|:---|:---:|
| **TC-007** | Búsqueda de artista válido ("Dua Lipa") | PE-Válida | Grilla de resultados renderiza tarjetas del artista. | ✅ PASS |
| **TC-008** | Búsqueda sin sentido ("xkqzlmpaowsi") | PE-Inválida | UI debe mostrar mensaje de "Sin resultados". | ❌ FAIL** |
| **TC-009** | Búsqueda regional ("Vinchos Ayacucho") | PE-Válida | Sistema recupera tracks andinos sin crashear. | ✅ PASS |
| **TC-010** | Cadena extrema de 800 caracteres | PE-Inválida | Sistema soporta el payload sin corromper el DOM. | ✅ PASS |
| **TC-011** | Búsqueda vacía / Solo espacios | Edge Case | Spotify no dispara peticiones XHR innecesarias. | ✅ PASS |
| **TC-012** | Inyección `XSS` y `SQL` combinada | Seguridad | Entradas sanitizadas. No se ejecutan `alert()`. | ✅ PASS |

---

## 5. Ejecución y Generación de Reportes
El framework está configurado para exportar evidencias en tres formatos estándar para canalizaciones (Pipelines) de CI/CD, cumpliendo con los requisitos de la Guía 07.

**Comando de ejecución y exportación:**
```bash
dotnet test --logger "trx;LogFileName=resultados.trx" --logger "html;LogFileName=resultados.html"