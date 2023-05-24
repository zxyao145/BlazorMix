
import { DragGesture } from "@use-gesture/vanilla"
import { stat } from "fs";

const getWidth = (element: HTMLElement) => {
    if (!element) return 0
    return element.offsetWidth
}

const nearest = (arr: number[], target: number) => {
    return arr.reduce((pre, cur) => {
        return Math.abs(pre - target) < Math.abs(cur - target) ? pre : cur
    })
}

enum DragState {
    end = 0,
    dragging = 1,
}

const sleep = (time: number) => {
    return new Promise(resolve =>
        setTimeout(resolve, time)
    )
}
const clsPrefix = "bm-swipe-action";
class SwipeAction {

    private componentRoot: HTMLElement;
    private trackEle: HTMLElement;
    private leftActionsEle: HTMLElement;
    private rightActionsEle: HTMLElement;
    private contentDiv: HTMLElement;


    private dotNetObj: any;
    private dragGesture: DragGesture | null = null;
    private isDraging: boolean = false;
    private x: number = 0;
    private dragCancel: (() => void) | null = null;

    constructor(dotNetObj: any, componentRoot: HTMLElement) {
        this.componentRoot = componentRoot;
        this.trackEle = componentRoot.querySelector(`.${clsPrefix}-track`) as HTMLElement;
        this.leftActionsEle = this.trackEle.querySelector(`.${clsPrefix}-actions-left`) as HTMLElement;
        this.rightActionsEle = this.trackEle.querySelector(`.${clsPrefix}-actions-right`) as HTMLElement;
        this.contentDiv = this.trackEle.querySelector(`.${clsPrefix}-content > div`) as HTMLElement;

        this.dotNetObj = dotNetObj;
        this.init();
    }

    init() {
        this.dragGesture = new DragGesture(this.componentRoot,
            state => {
                this.dragCancel = state.cancel;
            if (!state.intentional) {
                return;
            }
            if (state.pressed) {
                this.isDraging = true;
            }
            if (!this.isDraging) {
                return;
            }
            const [offsetX] = state.offset;
            if (state.last) {
                const leftWidth = this.getLeftWidth()
                const rightWidth = this.getRightWidth()
                let position = offsetX + state.velocity[0] * state.direction[0] * 50
                if (offsetX > 0) {
                    position = Math.max(0, position)
                } else if (offsetX < 0) {
                    position = Math.min(0, position)
                } else {
                    position = 0
                }
                const targetX = nearest([-rightWidth, 0, leftWidth], position)
                this.updateTrackTransform({
                    x: targetX,
                })
                if (targetX !== 0) {
                    this.dotNetObj.invokeMethodAsync("HandleActionsReveal", targetX > 0 ? 'left' : 'right' )
                }
                window.setTimeout(() => {
                    this.isDraging = false;
                })
            } else {
                this.updateTrackTransform({
                    x: offsetX,
                    immediate: true,
                })
            }
            }, {
                from: () => [this.x, 0],
                bounds: () => {
                    const leftWidth = this.getLeftWidth()
                    const rightWidth = this.getRightWidth()
                    return {
                        left: -rightWidth,
                        right: leftWidth,
                    }
                },
                // rubberband: true,
                axis: 'x',
                preventScroll: true,
                pointer: { touch: true },
                triggerAllEvents: true,
            })
    }

    getLeftWidth() {
        return getWidth(this.leftActionsEle);
    }
    getRightWidth() {
        return getWidth(this.rightActionsEle);
    }

    updateTrackTransform(obj: any) {
        this.x = obj.x;
        //console.log(this.trackEle, `translate3d(${this.x}px, 0px, 0px)`);
        this.trackEle.style.transform = `translate3d(${this.x}px, 0px, 0px)`;
        if (this.x != 0 && this.contentDiv.style.pointerEvents !== "none") {
            this.contentDiv.style.pointerEvents = "none";
        } else {
            this.contentDiv.style.pointerEvents = "auto";
        }
    }

    public async stopTransform() {
        // todo: more exquisite and elegant
        const positive = this.x > 0 ? 1 : -1;
        let curAbsX = Math.abs(this.x);
        let tension = 200;
        while (curAbsX > 0.05) {
            let plus = curAbsX * curAbsX / tension;
            plus = plus < 0.05 ? 0.05 : plus;
            plus = plus > 20 ? 20 : plus;
            curAbsX = curAbsX - plus;
            this.updateTrackTransform({ x: curAbsX * positive });
            await sleep(0);
        }

        if (this.dragCancel) {
            this.dragCancel();
        }
        this.isDraging = false;
    }

    dispose() {
        this.dragGesture?.destroy();
    }
}

export default SwipeAction;