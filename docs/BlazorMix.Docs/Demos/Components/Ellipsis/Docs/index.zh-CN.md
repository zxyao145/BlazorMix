# Ellipsis 文本省略

展示空间不足时，隐去部分内容并用“...”替代。

## 何时使用

- 文本内容长度或高度超过列宽或行高。
- 图表中空间有限，文本内容无法完全显示。
- 自适应调整时宽度变小。

<code-demo Src="Demos/Components/Ellipsis/Demos/Demo1"></code-demo>

## API

### 属性

> [xmldoc]

## FAQ

### Emoji

由于 System.Text.Json 不支持序列化 emoji，Ellipsis 组件暂时不支持含有 emoji 文字。