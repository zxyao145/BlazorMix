const getEle = (e: HTMLElement | string) => {
    if (typeof e === "string") {
        return document.querySelector(e as string) as HTMLElement;
    } else {
        return e;
    }
}

export const MoveEleTo = (
    ref: HTMLElement | string,
    container: HTMLElement | string
) => {
    let ele = getEle(ref);
    let c = getEle(container);
    if (ele && c) {
        c.append(ele);
    } 
}


let disableIndex = 0;
export const DisableBodyScroll = () => {
    disableIndex++;
    if (disableIndex === 1) {
        document.body.classList.add("overflow-hidden")
    }
}
export const EnableBodyScroll = () => {
    disableIndex--;
    if (disableIndex < 1) {
        document.body.classList.remove("overflow-hidden")
    }
}