
const clsPrefix = "bm-notice-bar";
const isAni = new Map<HTMLElement, boolean>();
const isInDelay = new Map<HTMLElement, boolean>();

/**
 * Bm.NoticeBar.startScroll
 * 
 * @param container
 * @param delayMs
 * @param speed
 */
export const startScroll = (container: HTMLElement, delayMs: number, speed: number) => {
    isInDelay.set(container, true);
    setTimeout(() => {
        isInDelay.delete(container);
        if (container) {
            const text = container.querySelector(`.${clsPrefix}-content-inner`) as HTMLElement;
            text.addEventListener("transitionend", () => {
                isAni.set(container, false);
                doScroll(container, speed);
            });
            doScroll(container, speed);
        }

    }, delayMs);
}


const doScroll = (container: HTMLElement, speed: number) => {
    if (isInDelay.has(container) || !container) {
        return;
    }

    const text = container.querySelector(`.${clsPrefix}-content-inner`) as HTMLElement;

    if (container.offsetWidth >= text.offsetWidth) {
        isAni.set(container, false);
        text.style.removeProperty('transition-duration')
        text.style.removeProperty('transform')
        return
    }

    if (isAni.has(container) && isAni.get(container)) {
        return;
    }
    const initial = !text.style.transform
    text.style.transitionDuration = '0s'
    if (initial) {
        text.style.transform = 'translateX(0)'
    } else {
        text.style.transform = `translateX(${container.offsetWidth}px)`
    }
    const distance = initial
        ? text.offsetWidth
        : container.offsetWidth + text.offsetWidth
    isAni.set(container, true);
    text.style.transitionDuration = `${Math.round(distance / speed)}s`
    text.style.transform = `translateX(-${text.offsetWidth}px)`
}


/**
 *  Bm.NoticeBar.endScroll
 * 
 * @param container
 */
export const endScroll = (container: HTMLElement) => {
    isInDelay.delete(container);
    isAni.delete(container);
}
