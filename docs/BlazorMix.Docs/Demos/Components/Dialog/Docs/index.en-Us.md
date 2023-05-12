# Dialog 对话框

用于重要信息的告知或操作的反馈，并附带少量的选项供用户进行操作。

## 何时使用

需要用户处理事务，又不希望跳转页面以致打断工作流程时，可以使用 Dialog 在当前页面正中打开一个浮层，承载相应的操作。


## Dialog

<code-demo Src="Demos/Components/Dialog/Demos/Demo1"></code-demo>

<code-demo Src="Demos/Components/Dialog/Demos/Demo2"></code-demo>

## API

### 属性

> [xmldoc](Dialog)

### DialogAction / DialogActionItem
> [xmldoc](DialogActionItem)

## 指令式
可以通过指令式的方式使用 Dialog：

### DialogService.Show

```csharp
var dialogRef = DialogService.Show(new DialogOption{})
```

可以通过调用 DialogService 的 Show 方法直接打开对话框，其中 DialogOption 参数的类型同上表，但不支持传入 Visible 属性。

当对话框被关闭后，组件实例会自动销毁。

`Show` 方法返回一个 `DialogRef` 对象，可以通过 `DialogRef.Close` 关闭显示中的 Dialog 对象.

`Show` 只是一个很基础的方法，在实际业务中，更为常用的是下面的 Alert 和 Confirm 方法：

### DialogService.alert

`Alert` 接受的参数同 `Show`(`DialogAlertOption`)，但不支持 `CloseOnAction` `Actions` 属性，它的返回值是 Task，这个 Task 会等待 Action 按钮的点击。此外，它还额外支持以下属性：

此外，它还额外支持以下属性：



| 属性 | 说明 | 类型 | 默认值 |
| --- | --- | --- | --- |
| ConfirmText | 确认按钮的内容 | `StringOrRenderFragment` | `'我知道了'` |
| OnConfirm | 点击确认按钮时触发 | `Func<DialogActionEventArgs, Task>` | - |

### DialogService.Confirm

`Confirm` 接受的参数同 `Show`(`DialogConfirmOption`)，但不支持 `CloseOnAction` `Actions` 属性，它的返回值是 Task<bool>， bool 用来表明 点击了确定按钮还是取消按钮。
需要注意的是，这个 Task<bool> 同样会等待 Action 按钮的点击。

此外，它还额外支持以下属性：

| 属性        | 说明               | 类型                          | 默认值   |
| ----------- | ------------------ | ----------------------------- | -------- |
| CancelText  | 取消按钮的内容     | `StringOrRenderFragment`                   | `'取消'` |
| ConfirmText | 确认按钮的内容     | `StringOrRenderFragment`                   | `'确认'` |
| OnCancel    | 点击取消按钮时触发 | `Func<DialogActionEventArgs, Task>` | -        |
| OnConfirm   | 点击确认按钮时触发 | `Func<DialogActionEventArgs, Task>` | -        |

### DialogService.Clear
可以通过调用 DialogService 上的 clear 方法关闭所有打开的对话框，通常用于路由监听中，处理路由前进、后退不能关闭对话框的问题。
