# Toast 轻提示

对操作结果的轻量级反馈，无需用户操作即可自行消失。

## 何时使用

适用于页面内容的变化不能直接反应操作结果时使用。

<code-demo Src="Demos/Components/Toast/Demos/Demo1"></code-demo>

## Toast

## 指令式 API

Toast 只支持指令式调用。

```csharp
@inject ToastService ToastService;
ToastService.Show(new ToastOption(){});
```

### ToastService.Show

`Show` 方法支持传入一个 `ToastOption` 对象，它包含了以下这些属性：

> [xmldoc](ToastOption)

> 同一时间只允许弹出一个轻提示，新出现的 Toast 会将之前正在显示中的 Toast 挤掉。

`Show` 方法返回一个 `ToastRef` 对象，可以通过 `ToastRef.Close` 关闭显示中的 Toast 对象.

### ToastService.Clear

关闭所有显示中的 Toast。

### ToastService.Config

全局配置，支持配置 duration、position 和 maskClickable。

```csharp
public void Config(
        int? durationMs = null,
        ToastPosition? position = null,
        bool? maskClickable = null
        )
```