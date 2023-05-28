# Image 图片

可预览的图片。

# 何时使用

- 需要展示图片时使用。
- 加载大图时显示 loading 或加载失败时容错处理。

<code-demo Src="Demos/Components/Image/Demos/Demo1"></code-demo>

<code-demo Src="Demos/Components/Image/Demos/Demo2"></code-demo>

## API

### 属性

> [xmldoc]

width height 属性其实和 CSS 变量 `--width` `--height` 并不冲突，这些组件属性其实就是基于 CSS 变量实现的，只是 CSS 变量的一种快捷设置方式。

### CSS 变量

| 属性     | 说明     | 默认值 | 全局变量             |
| -------- | -------- | ------ | -------------------- |
| --height | 图片高度 | `auto` | `--adm-image-height` |
| --width  | 图片宽度 | `auto` | `--adm-image-width`  |

