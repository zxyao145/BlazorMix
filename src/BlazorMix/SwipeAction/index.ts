
import SwipeAction from "./SwipeAction"

const delObs = new WeakMap<Node, () => void>();
const observer = new MutationObserver((recordArr: MutationRecord[]) => {
    if (recordArr.length > 0 && recordArr[0].type === 'childList') {
        if (recordArr[0].removedNodes.length > 0) {
            const node = recordArr[0].removedNodes[0];
            const func = delObs.get(node);
            if (func) {
                func();
            }
            delObs.delete(node);
        }
    }
});
const insMap = new Map<any, SwipeAction>();

export const init = (dotObj: any, closeOnTouchOutside: boolean, container: HTMLElement) => {
    const id = dotObj._id;
    if (insMap.has(id)) {
        return;
    }

    const sa = new SwipeAction(dotObj, closeOnTouchOutside, container);
    delObs.set(container, () => {
        sa.dispose();
    })
    var observerOptions = {
        childList: true,
        subtree: true,
    }
    observer.observe(container, observerOptions);

    insMap.set(id, sa)
    return sa;
}

document.addEventListener('touchstart', (e) => {
    for (let kvArr of insMap) {
        kvArr[1].handleOnTouchOutsideEvent(e);
    }
});
