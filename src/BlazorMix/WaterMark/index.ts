import { pxToNumber } from "../../util"

export interface WaterMark {
    gapX: number;
    gapY: number;
    //   zIndex: number;
    width: number;
    height: number;
    rotate: number;
    image: string;
    imageWidth: number;
    imageHeight: number;
    content: string[];
    fontColor: string;
    fontStyle: string;
    fontWeight: number;
    fontFamily: string;
    fontSize: string;
    //   fullPage: boolean;
}

export const render = (root: HTMLElement, props: WaterMark) => {
    const setBase64Url = (base64Url: string) => {
        root.style.backgroundImage = `url(${base64Url})`;
    };

    const canvas = document.createElement("canvas");
    const ratio = window.devicePixelRatio;
    const ctx = canvas.getContext("2d");

    const canvasWidth = `${(props.gapX + props.width) * ratio}px`;
    const canvasHeight = `${(props.gapY + props.height) * ratio}px`;

    const markWidth = props.width * ratio;
    const markHeight = props.height * ratio;

    canvas.setAttribute("width", canvasWidth);
    canvas.setAttribute("height", canvasHeight);

    if (ctx) {
        const content = props.content;
        if (props.image) {
            ctx.translate(markWidth / 2, markHeight / 2);
            ctx.rotate((Math.PI / 180) * Number(props.rotate));

            const img = new Image();
            img.crossOrigin = "anonymous";
            img.referrerPolicy = "no-referrer";
            img.onload = () => {
                ctx.drawImage(
                    img,
                    (-props.imageWidth * ratio) / 2,
                    (-props.imageHeight * ratio) / 2,
                    props.imageWidth * ratio,
                    props.imageHeight * ratio
                );
                ctx.restore();
                setBase64Url(canvas.toDataURL());
            };
            img.src = props.image;
        } else if (content) {
            ctx.textBaseline = "middle";
            ctx.textAlign = "center";
            // 文字绕中间旋转
            ctx.translate(markWidth / 2, markHeight / 2);
            ctx.rotate((Math.PI / 180) * Number(props.rotate));

            const markSize = pxToNumber(props.fontSize) * ratio;
            ctx.font = `${props.fontStyle} normal ${props.fontWeight} ${markSize}px/${markHeight}px ${props.fontFamily}`;
            ctx.fillStyle = props.fontColor;
            if (content.length === 1) {
                ctx.fillText(content[0], 0, 0);
            } else {
                content.forEach((item: string, index: number) =>
                    ctx.fillText(item, 0, index * markSize)
                );
            }
            ctx.restore();
            setBase64Url(canvas.toDataURL());
        }
    } else {
        throw new Error("Canvas is not supported in the current environment");
    }
};
