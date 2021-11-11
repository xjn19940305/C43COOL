import moment from 'moment'
var Tools = {
    getImage(fileId) {
        // var host = window.document.location.origin
        var host = process.env.VUE_APP_BASE_URL
        return host + '/api/File/GetFileContent/' + fileId
    },
    formatUtc(time, fmt) {
        return moment.utc(time).utcOffset(8).format(fmt);
    }
}
export default Tools