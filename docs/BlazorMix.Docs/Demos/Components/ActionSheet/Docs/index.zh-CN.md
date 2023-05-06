# ActionSheet 动作面板

从底部弹出的动作菜单面板。


## 何时使用

由用户操作触发，提供一组与当前场景操作相关的两个或多个选项，让用户在不离场的情况下完成操作。

<code-demo Src="Demos/Components/ActionSheet/Demos/Demo1"></code-demo>

## API

### 属性

> [xmldoc]

### ActionInfo

| 属性        | 说明           | 类型               | 默认值  |
| ----------- | -------------- | ------------------ | ------- |
| Danger      | 是否为危险状态   | `bool`                      | `false` |
| Description | 描述           | `StringOrRenderFragment`    | -       |
| Disabled    | 是否为禁用状态   | `bool`                      | `false` |
| Key         | 唯一标记        | `string`                      | -       |
| OnClick     | 点击时触发      | `Func<ValueTask>?`             | -       |
| Text        | 标题           | `StringOrRenderFragment`        | -       |
| Bold        | 标题是否加粗     | `bool`                      | `false` |

### 服务

可以通过服务的方式使用 ActionSheet：

```c#
// [inject] ActionSheetService ActionSheetService
var eleRef = ActionSheetService.Show(new ActionSheetOption { Actions = actions }));
```

当动作面板被关闭后，组件实例会自动销毁。

`Show` 方法的返回值 `ActionSheetRef` 为一个组件控制器，包含以下属性：


| 属性  | 说明         | 类型         |
| ----- | ------------ | ------------ |
| close | 关闭动作面板 | `() => ValueTask` |