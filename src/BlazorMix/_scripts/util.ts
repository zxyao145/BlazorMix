export function pxToNumber(value: string | null): number {
    if (!value) return 0
    const match = value.match(/^\d*(\.\d*)?/)
    return match ? Number(match[0]) : 0
}