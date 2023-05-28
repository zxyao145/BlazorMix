# BlazorMix
A Blazor Mobile UI components lib.

[![Nuget (with prereleases)](https://img.shields.io/nuget/vpre/BlazorMix)](https://www.nuget.org/packages/BlazorMix/)

[![Nuget (with prereleases)](https://img.shields.io/nuget/vpre/BlazorMix.Core)](https://www.nuget.org/packages/BlazorMix.Core/)

[BlazorMix Docs](https://blazormix.com/)


## What is "Mix"?

BlazorMix referenced multiple mobile component libraries and "mixed" multiple components library, which is why Mix came into being.


## Usage Guidelines

### 1. install package

```bash
dotnet add package BlazorMix
```

### 2. Register the services 

```csharp
builder.Services.AddBlazorMix();
```

### 3. Link the static files

```html
<link href="_content/BlazorMix/dist/style.css" rel="stylesheet" />
<script src="_content/BlazorMix/dist/index.min.js"></script>
```

###  4. Add namespace in `_Imports.razor`

```razor
@using BlazorMix;
```


### 5. Add the `<BlazorMixContainer />`  component in `App.razor`

```razor
<Router AppAssembly="@typeof(App).Assembly">
   ...
</Router>

+ <BlazorMixContainer />
```

### 6. Use component in `.razor` file.
```razor
<Button Color="@ButtonColor.Primary">Hello World!</Button>
```

## OS
During the construction process of this component and documentation, the React/Blazor components referenced include:

*   Ant Design Mobile
*   Zarm Design
*   Nut UI
*   Tdesign Mobile
*   TDesignBlazor
*   Ant Design of Blazor
*   MASA.Blazor
*   Bootstrap

Other technologies/frameworks/components:

*   typescript
*   scss
*   vite
*   prism.js
*   IconPark.Blazor
*   QRCoder
