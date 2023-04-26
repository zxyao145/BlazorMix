
export const MoveEleTo = (ref: HTMLElement, container: HTMLElement | string) => {
    let c: HTMLElement;
    if (typeof container === "string") {
        c = document.querySelector(container as string) as HTMLElement;
    } else {
        c = container as HTMLElement;
    }

    c.append(ref);
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