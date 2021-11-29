import { request, METHOD } from '@/utils/request'
import { ROLE } from '@/services/api'
var RoleApi = {
    getList(params) {
        params = params || {}
        // var RoleName = params.RoleName || ''
        return request(
            `${ROLE.Query}`,
            METHOD.GET,
            params
        )
    },
    Create(data) {
        return request(
            `${ROLE.Create}`,
            METHOD.POST,
            data
        )
    },
    Modify(data) {
        return request(
            `${ROLE.Modify}`,
            METHOD.PUT,
            data
        )
    },
    Get(params) {
        return request(
            `${ROLE.Get}`,
            METHOD.Get,
            params
        )
    },
    Delete(params) {
        return request(
            `${ROLE.Delete}`,
            METHOD.DELETE,
            params
        )
    },
    GetRoleMenus(params) {
        return request(
            `${ROLE.GetRoleMenus}`,
            METHOD.Get,
            params
        )
    },
    AuthorizeRoleMenu(data) {
        return request(
            `${ROLE.AuthorizeRoleMenu}`,
            METHOD.PUT,
            data
        )
    }
}
export default RoleApi
