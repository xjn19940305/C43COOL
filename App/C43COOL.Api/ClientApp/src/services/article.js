import { request, METHOD } from '@/utils/request'
const BASE_URL = `${process.env.VUE_APP_BASE_URL}/api/mgt/Article`
var ArticleApi = {
    Query(params) {
        params = params || {}
        // var RoleName = params.RoleName || ''
        return request(
            `${BASE_URL}/Query`,
            METHOD.GET,
            params
        )
    },
    Create(data) {
        return request(
            `${BASE_URL}/Create`,
            METHOD.POST,
            data
        )
    },
    Modify(data) {
        return request(
            `${BASE_URL}/Update`,
            METHOD.PUT,
            data
        )
    },
    Get(id) {
        return request(
            `${BASE_URL}/Get/${id}`,
            METHOD.Get
        )
    },
    Delete(params) {
        return request(
            `${BASE_URL}/Delete`,
            METHOD.DELETE,
            params
        )
    }
}
export default ArticleApi
