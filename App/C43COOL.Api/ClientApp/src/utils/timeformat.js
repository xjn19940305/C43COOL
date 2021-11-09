import moment from 'moment'
/*
 * Utc 转 北京时间
 */
export function formatUtc(time, fmt) {
    return moment.utc(time).utcOffset(8).format(fmt);
}

/*
 * Utc 转 北京时间
 */
export function formatUtcTime(time) {
    return moment.utc(time).utcOffset(8);
}