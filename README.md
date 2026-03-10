#  Proyecto Rick Y Morty - Juan Usuga

![Rick and Morty](https://e7.pngegg.com/pngimages/479/224/png-clipart-rick-and-morty-rick-sanchez-rick-and-morty-season-3-adult-swim-rick-and-morty-season-2-episode-rick-and-morty-grass-fictional-character.png)

## Descripción General

Este proyecto es una aplicación web interactiva desarrollada en **Blazor .NET 10** que consume la [API de Rick and Morty](https://rickandmortyapi.com/) para mostrar información de los personajes de la serie. La aplicación permite a los usuarios explorar personajes, filtrarlos por diferentes criterios, y votar mediante un sistema de likes/dislikes.

## Índice
- [Características Principales](#características-principales)
- [Instalación](#instrucciones-para-ejecutar-la-app)
- [Uso](#uso)
- [Tecnologías](#tecnologías)
- [Contribución](#contribución)

---

###     Características Principales

- **Galería de personajes** en formato grid responsivo
- **Sistema de votación** (likes/dislikes) con puntaje en tiempo real
- **Filtros avanzados** por nombre, especie y estado
- **Paginación completa** con navegación intuitiva
- **Diseño moderno** con animaciones y efectos visuales
- **Arquitectura limpia** siguiendo principios SOLID

---

## Instrucciones para Ejecutar la App

###     Requisitos Previos

- **.NET SDK 10.0** o superior
- Un editor de código (Visual Studio, VS Code, o Rider)
- Navegador web moderno

###     Instalación

1. **Clonar el repositorio:**
   ```bash
   git clone https://github.com/JuanMa594/ProyectoRickYMorty.git
   cd ProyectoRickYMorty
   ```

2. **Restaurar dependencias:**
   ```bash
   dotnet restore
   ```

3. **Ejecutar la aplicación:**
   ```bash
   dotnet run
   ```

4. **Abrir en el navegador:**
   ```
   https://localhost:5001
   ```
   O la URL que se muestre en la consola.


---

## Cómo se Consumió la API

###     Endpoint Utilizado

La aplicación consume el endpoint de **Characters** de la API de Rick and Morty:

```
Base URL: https://rickandmortyapi.com/api/character
```

###     Implementación

#### 1. Configuración de HttpClient

En `Program.cs`:

```csharp
builder.Services.AddHttpClient("RickAndMortyApi", client =>
{
    client.BaseAddress = new Uri("https://rickandmortyapi.com/api/character");
    client.DefaultRequestHeaders.Add("Accept", "application/json");
});
```

#### 2. Servicio de Abstracción

Se implementó una capa de servicio siguiendo el patrón **Repository**:

```csharp
public interface IRickAndMortyService
{
    Task<ApiResponse<Character>> GetCharactersAsync(int page = 1);
    Task<Character?> GetByIdAsync(int id);
    Task<List<Character>> GetMultipleCharactersAsync(params int[] ids);
    Task<ApiResponse<Character>> FilterCharactersAsync(...);
}
```

#### 3. Operaciones Disponibles

| Método | Descripción | Endpoint |
|--------|-------------|----------|
| `GetCharactersAsync(page)` | Obtiene personajes paginados | `GET /character?page={page}` |
| `GetByIdAsync(id)` | Obtiene un personaje por ID | `GET /character/{id}` |
| `GetMultipleCharactersAsync(ids)` | Obtiene múltiples personajes | `GET /character/{id1},{id2}` |
| `FilterCharactersAsync(...)` | Filtra personajes | `GET /character?name=...&status=...` |

#### 4. Manejo de Errores

- **404**: Se maneja devolviendo `null` o listas vacías
- **Errores de red**: Se capturan con `try-catch` y se registran
- **Paginación**: Control de límites de página

---

## Estructura del Código

```
ProyectoRickYMorty/
│
├── Components/
│   ├── Layout/
│   │   ├── NavMenu.razor           # Menú de navegación lateral
│   │   ├── MainLayout.razor        # Layout principal
|   |   └── MainLayout.razor.css    # Personalización CSS del Layout principal
│   │
│   ├── Pages/
│   │   ├── Home.razor              # Página de inicio
│   │   ├── Characters.razor        # Galería de personajes (página principal)
|   │   ├── Error.razor             # Página de error, en caso de un error inesperado
│   │   └── NotFound.razor          # Error 404
│   │
|   ├── _Imports.razor              # Manejo de imports globales
│   └── App.razor                   # Componente raíz
|
├── Models/
│   ├── ApiResponse.cs              # Modelo de respuesta paginada e Información de Paginación
│   └── Character.cs                # Modelo del personaje y ubicación
│
├── Services/
│   ├── IRickAndMortyService.cs     # Interfaz del servicio
│   └── RickAndMortyService.cs      # Implementación del servicio
│
├── wwwroot/
│   ├── css/
│   │   └── app.css                 # Estilos y personalización de toda la aplicación
│   │
|   ├── lib/bootsrap/dist           # Librería de Bootstrap y CSS  
|   │
|   ├── Fonts/                      
|   |   └── get_scwifty.ttf         # Tipo de letra, estilo Rick Y Morty
|   |
│   └── LogoPagina.png              # Logo usado para la aplicación
│
├── Program.cs                      # Configuración de la aplicación
└── appsettings.json                # Configuración general
```
*NOTA: Ahí se hizo la estructura general del proyecto, colocando los archivos o carpetas que se añadieron, modificaron o se programaron, sin contar archivos o carpetas adicionales que sirven más que todo para una correcta funcionalidad del proyecto.*

###   Componentes Clave

#### `RickAndMortyService.cs`
- Encapsula toda la lógica de comunicación con la API
- Maneja errores y excepciones
- Configura opciones de JSON (camelCase)
- Implementa `IHttpClientFactory` para gestión eficiente de conexiones

#### `Characters.razor`
- Página donde se muestran los personajes de Rick Y Morty
- Componente principal con renderizado interactivo (`@rendermode InteractiveServer`)
- Gestiona estado de paginación, filtros y votaciones
- Implementa UI con Bootstrap y CSS personalizado
- Maneja eventos de usuario (clicks, filtros, navegación)

#### `NavMenu.razor`
- Menú lateral con navegación en las distintas páginas
- Animaciones y efectos hover
- Diseño tipo 'liquid-glass'

---

## Funcionalidades Adicionales

###     1. Filtros Avanzados

La aplicación implementa tres tipos de filtros que se pueden combinar:

- **Por Nombre**: Búsqueda parcial (ej: "rick" encuentra "Rick Sanchez")
- **Por Estado**: Selector con opciones (Vivo, Muerto, Desconocido)
- **Por Especie**: Búsqueda parcial (ej: "human", "alien")

**Características:**
- Filtros independientes y combinables
- Botón "Buscar" para aplicar filtros y Botón "Limpiar" para resetear
- Mensaje dinámico mostrando resultados y filtros activos
- Manejo de resultados vacíos (404)

###     2. Animaciones Suaves y Estilo Visual Personalizado

#### Menú de Navegación
- **Hover effect**: Deslizamiento hacia la derecha + cambio de fondo
- **Ripple effect**: Onda al hacer clic
- **Transiciones**: Animación `cubic-bezier` para movimiento fluido
- **Estado activo**: Barra de color lateral con gradiente verdeoscuro-verdeaguamarina

#### Tarjetas de Personajes
- **Hover**: Elevación con `translateY(-5px)` y sombra aumentada
- **Transiciones suaves**: 0.3s en todas las interacciones
- **Efectos en botones**: Scale y sombra al activarse
- **Loading state**: Spinner animado

#### Paginación
- **Botones diferenciados**: Anterior/Siguiente con fondo gris oscuro
- **Botones de borde**: Primera/Última con borde más grueso e ícono grande
- **Hover states**: Cambios de color suaves
- **Página activa**: Resaltada con fondo azul

---

## Decisiones Técnicas

### 1. **Blazor InteractiveServer vs InteractiveAuto**

**Decisión:** Se utilizó `@rendermode InteractiveServer`

**Razones:**
- ✅ Más simple de configurar en .NET 10
- ✅ No requiere descargar WASM al cliente
- ✅ Menor peso inicial de la aplicación
- ✅ Suficiente para las necesidades del proyecto (API externa)
- ✅ Excelente para aplicaciones con usuarios concurrentes moderados

### 2. **HttpClientFactory en lugar de HttpClient directo**

**Decisión:** Implementar `IHttpClientFactory`

**Razones:**
- ✅ Evita agotamiento de sockets
- ✅ Gestión automática del ciclo de vida
- ✅ Mejor rendimiento en aplicaciones de producción
- ✅ Facilita testing con clientes mock
- ✅ Configuración centralizada

### 3. **Patrón Repository con Interfaz**

**Decisión:** Crear `IRickAndMortyService` + implementación

**Razones:**
- ✅ Inversión de dependencias (SOLID)
- ✅ Facilita testing unitario
- ✅ Permite cambiar implementación sin afectar componentes
- ✅ Encapsula lógica de negocio
- ✅ Mejor mantenibilidad

### 4. **Manejo de Estado con Diccionarios**

**Decisión:** Usar `Dictionary<int, int>` para puntajes

**Razones:**
- ✅ Acceso O(1) por ID de personaje (Acceso instantáneo) 
- ✅ Persiste durante la sesión de navegación
- ✅ Simple y eficiente para estado local
- ❌ No persiste entre recargas (Decisión consciente para simplicidad. Se puede realizar en un futuro)

### 5. **Bootstrap + CSS Personalizado**

**Decisión:** Combinar Bootstrap con Personalización CSS

**Razones:**
- ✅ Bootstrap proporciona una base personalizada de la página
- ✅ Personalización CSS para identidad visual única
- ✅ Evita look "genérico" de frameworks
- ✅ Mantiene consistencia en componentes
- ✅ Fácil de mantener y extender

### 6. **JsonSerializerOptions con camelCase**

**Decisión:** Configurar `PropertyNamingPolicy.CamelCase`

**Razones:**
- ✅ La API de Rick and Morty usa camelCase
- ✅ Convención estándar en APIs REST
- ✅ Evita errores de deserialización
- ✅ Configuración centralizada en el servicio

### 7. **Paginación Client-Side**

**Decisión:** No implementar infinite scroll, usar paginación tradicional

**Razones:**
- ✅ Mejor UX, debido a la cantidad tan grande personajes (826 personajes)
- ✅ Control explícito del usuario
- ✅ Menor consumo de memoria
- ✅ Más accesible
- ✅ Coincide con capacidades de la API

### 8. **Filtros con Peticiones Nuevas vs Cache Local**

**Decisión:** Hacer petición nueva a la API por cada filtro

**Razones:**
- ✅ Resultados siempre actualizados
- ✅ Menor uso de memoria en el cliente
- ✅ Aprovecha capacidades nativas de la API
- ✅ Mejor para datos que cambian frecuentemente
- ❌ Más peticiones de red (aceptable dado que la API es gratuita y rápida)

---

## Tecnologías Utilizadas

| Tecnología | Versión | Propósito |
|------------|---------|-----------|
| .NET | 10.0 | Framework principal |
| Blazor | .NET 10 | Frontend interactivo |
| Bootstrap | 5.3.8 | Framework CSS |
| Bootstrap Icons | 1.11.0 | Iconografía |
| C# | 12.0 | Lenguaje de programación |
| Rick and Morty API | v1 | Fuente de datos |

---

## Capturas de Pantalla

### Galería de Personajes
![Galería](docs/Captura%20de%20pantalla(1).png)

### Filtros
![Filtros](docs/Captura%20de%20pantalla(2).png)

### Menú de Navegación
![Menú](docs/Captura%20de%20pantalla%20(3).png)

---

## Pruebas Funcionales
**Se agregó un archivo llamado Pruebas en la carpeta docs, en donde se puede encontrar las Pruebas funcionales para el proyecto**

---

## 🔮 Mejoras Futuras

- [ ] Implementar persistencia de puntajes (LocalStorage)
- [ ] Agregar página de detalles por personaje, en donde el usuario pueda acceder a una página con cada detalle del personaje, más fotos del personaje, y en qué episodio(s) aparece
- [ ] Agregar modo oscuro
- [ ] Implementar PWA (Progressive Web App)
- [ ] Hacerla Responsiva para todo tipo de dispositivos

---

## Autor

**Juan Manuel Usuga Galeano**
