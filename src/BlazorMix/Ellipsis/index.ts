import { pxToNumber } from "../../util"

//document.addEventListener('touchstart', (e) => {
//    for (let kvArr of insMap) {
//        kvArr[1].handleOnTouchOutsideEvent(e);
//    }
//});


interface EllipsisProps {
    content: string
    direction?: 'start' | 'end' | 'middle'
    rows: number
    expandText?: string
    collapseText?: string
    expanded?: boolean
}

interface EllipsisedValue {
    leading?: string
    tailing?: string
}


const getSubString = (content: string, start: number, end: number) => {
    return content.slice(start, end)
}

export const calcEllipsised = (dotNetObj: any, root: HTMLElement, props: EllipsisProps, content: string) => {
    if (!root) return;
    if (!root.offsetParent) return;

    const originStyle = window.getComputedStyle(root)
    const container = document.createElement('div')
    const styleNames: string[] = Array.prototype.slice.apply(originStyle)
    styleNames.forEach(name => {
        container.style.setProperty(name, originStyle.getPropertyValue(name))
    })
    container.style.position = 'fixed'
    container.style.left = '999999px'
    container.style.top = '999999px'
    container.style.zIndex = '-1000'
    container.style.height = 'auto'
    container.style.minHeight = 'auto'
    container.style.maxHeight = 'auto'
    container.style.textOverflow = 'clip'
    container.style.whiteSpace = 'normal'
    container.style.webkitLineClamp = 'unset'
    container.style.display = 'block'

    const lineHeight = pxToNumber(originStyle.lineHeight)
    const maxHeight = Math.floor(
        lineHeight * (props.rows + 0.5) +
        pxToNumber(originStyle.paddingTop) +
        pxToNumber(originStyle.paddingBottom)
    )
    container.innerText = content
    document.body.appendChild(container)

    let result = {
        "exceeded": true,
        "leading": "",
        "tailing": "",
    }; 
    if (container.offsetHeight <= maxHeight) {
        result.exceeded = false;
    } else {
        const end = content.length
        const actionText = props.expanded ? props.collapseText : props.expandText

        const check = (left: number, right: number): EllipsisedValue => {
            if (right - left <= 1) {
                if (props.direction === 'end') {
                    return {
                        leading: getSubString(content, 0, left) + '...',
                    }
                } else {
                    return {
                        tailing: '...' + getSubString(content, right, end),
                    }
                }
            }
            const middle = Math.round((left + right) / 2)
            if (props.direction === 'end') {
                container.innerText = getSubString(content, 0, middle) + '...' + actionText
            } else {
                container.innerText = actionText + '...' + getSubString(content, middle, end)
            }
            if (container.offsetHeight <= maxHeight) {
                if (props.direction === 'end') {
                    return check(middle, right)
                } else {
                    return check(left, middle)
                }
            } else {
                if (props.direction === 'end') {
                    return check(left, middle)
                } else {
                    return check(middle, right)
                }
            }
        }

        const checkMiddle = (
            leftPart: [number, number],
            rightPart: [number, number]
        ): EllipsisedValue => {
            if (
                leftPart[1] - leftPart[0] <= 1 &&
                rightPart[1] - rightPart[0] <= 1
            ) {
                return {
                    leading: getSubString(content, 0, leftPart[0]) + '...',
                    tailing: '...' + getSubString(content, rightPart[1], end),
                }
            }
            const leftPartMiddle = Math.floor((leftPart[0] + leftPart[1]) / 2)
            const rightPartMiddle = Math.ceil((rightPart[0] + rightPart[1]) / 2)
            container.innerText =
                getSubString(content, 0, leftPartMiddle) +
                '...' +
                actionText +
                '...' +
            getSubString(content, rightPartMiddle, end)
            if (container.offsetHeight <= maxHeight) {
                return checkMiddle(
                    [leftPartMiddle, leftPart[1]],
                    [rightPart[0], rightPartMiddle]
                )
            } else {
                return checkMiddle(
                    [leftPart[0], leftPartMiddle],
                    [rightPartMiddle, rightPart[1]]
                )
            }
        }

        const middle = Math.floor((0 + end) / 2)
        const ellipsised =
            props.direction === 'middle'
                ? checkMiddle([0, middle], [middle, end])
                : check(0, end)

        result.leading = ellipsised.leading ?? "";
        result.tailing = ellipsised.tailing ?? "";
    }
    document.body.removeChild(container);

    return result;
}
