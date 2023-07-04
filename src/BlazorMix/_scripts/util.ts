
export const getEle = (e: HTMLElement | string) => {
    if (typeof e === "string") {
        return document.querySelector(e as string) as HTMLElement;
    } else {
        return e;
    }
}


export function pxToNumber(value: string | null): number {
    if (!value) return 0
    const match = value.match(/^\d*(\.\d*)?/)
    return match ? Number(match[0]) : 0
}

export const nearest = (arr: number[], target: number) => {
    return arr.reduce((pre, cur) => {
        return Math.abs(pre - target) < Math.abs(cur - target) ? pre : cur;
    });
};

export const canUseDom = !!(
    typeof window !== 'undefined' &&
    typeof document !== 'undefined' &&
    window.document &&
    window.document.createElement
)