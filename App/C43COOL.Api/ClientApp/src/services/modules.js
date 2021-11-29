import { request, METHOD } from '@/utils/request'
import { MODULE } from '@/services/api'
var ModuleApi = {
    Query(params) {
        params = params || {}
        // var RoleName = params.RoleName || ''
        return request(
            `${MODULE.Query}`,
            METHOD.GET,
            params
        )
    },
    Create(data) {
        return request(
            `${MODULE.Create}`,
            METHOD.POST,
            data
        )
    },
    Modify(data) {
        return request(
            `${MODULE.Modify}`,
            METHOD.PUT,
            data
        )
    },
    Get(params) {
        return request(
            `${MODULE.Get}`,
            METHOD.Get,
            params
        )
    },
    Delete(params) {
        return request(
            `${MODULE.Delete}`,
            METHOD.DELETE,
            params
        )
    }
}
export default ModuleApi