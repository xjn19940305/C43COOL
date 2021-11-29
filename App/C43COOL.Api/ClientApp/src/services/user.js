import { LOGIN, ROUTES, USERINFO, GETMODULESFROMTOKEN, USER } from '@/services/api'
import { request, METHOD, removeAuthorization } from '@/utils/request'

/**
 * 登录服务
 * @param name 账户名
 * @param password 账户密码
 * @returns {Promise<AxiosResponse<T>>}
 */
export async function login(Account, Password) {
  return request(LOGIN, METHOD.POST, {
    Account,
    Password
  })
}
export async function getUserInfo() {
  return request(USERINFO, METHOD.GET)
}
export async function getModules() {
  return request(GETMODULESFROMTOKEN, METHOD.GET)
}
export async function getRoutesConfig() {
  return request(ROUTES, METHOD.GET)
}

/**
 * 退出登录
 */
export function logout() {
  localStorage.removeItem(process.env.VUE_APP_ROUTES_KEY)
  localStorage.removeItem(process.env.VUE_APP_PERMISSIONS_KEY)
  localStorage.removeItem(process.env.VUE_APP_ROLES_KEY)
  removeAuthorization()
}
export var UserApi = {
  getList(params) {
    params = params || {}
    // var RoleName = params.RoleName || ''
    return request(
      `${USER.Query}`,
      METHOD.GET,
      params
    )
  },
  Create(data) {
    return request(
      `${USER.Create}`,
      METHOD.POST,
      data
    )
  },
  Modify(data) {
    return request(
      `${USER.Modify}`,
      METHOD.PUT,
      data
    )
  },
  Get(params) {
    return request(
      `${USER.Get}`,
      METHOD.Get,
      params
    )
  },
  Delete(params) {
    return request(
      `${USER.Delete}`,
      METHOD.DELETE,
      params
    )
  },
  GetBindRoleList(params) {
    return request(
      `${USER.GetRoleList}`,
      METHOD.Get,
      params
    )
  },
  SaveUserRole(data) {
    return request(
      `${USER.SaveUserRole}`,
      METHOD.POST,
      data
    )
  }
}

export default {
  login,
  logout,
  getRoutesConfig,
  UserApi
}
