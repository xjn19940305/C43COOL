//跨域代理前缀
// const API_PROXY_PREFIX='/api'
// const BASE_URL = process.env.NODE_ENV === 'production' ? process.env.VUE_APP_API_BASE_URL : ''
const BASE_URL = process.env.VUE_APP_BASE_URL
console.log(process.env)
module.exports = {
  LOGIN: `${BASE_URL}/api/mgt/User/SignInWithPassword`,
  USERINFO: `${BASE_URL}/api/mgt/User/GetUserInfo`,
  GETMODULESFROMTOKEN: `${BASE_URL}/api/mgt/User/GetModules`,
  ROUTES: `${BASE_URL}/routes`,
  GOODS: `${BASE_URL}/goods`,
  GOODS_COLUMNS: `${BASE_URL}/columns`,
  ROLE: {
    Query: `${BASE_URL}/api/mgt/Role/Query`,
    Create: `${BASE_URL}/api/mgt/Role/Create`,
    Modify: `${BASE_URL}/api/mgt/Role/Modify`,
    Get: `${BASE_URL}/api/mgt/Role/Get`,
    Delete: `${BASE_URL}/api/mgt/Role/Delete`,
    GetRoleMenus:`${BASE_URL}/api/mgt/Role/GetRoleMenus`,
    AuthorizeRoleMenu:`${BASE_URL}/api/mgt/Role/AuthorizeRoleMenu`,
  },
  MODULE: {
    Query: `${BASE_URL}/api/mgt/Modules/Query`,
    Create: `${BASE_URL}/api/mgt/Modules/Create`,
    Modify: `${BASE_URL}/api/mgt/Modules/Modify`,
    Get: `${BASE_URL}/api/mgt/Modules/Get`,
    Delete: `${BASE_URL}/api/mgt/Modules/Delete`,
  },
  USER:{
    Query: `${BASE_URL}/api/mgt/User/Query`,
    Create: `${BASE_URL}/api/mgt/User/Create`,
    Modify: `${BASE_URL}/api/mgt/User/Modify`,
    Get: `${BASE_URL}/api/mgt/User/Get`,
    Delete: `${BASE_URL}/api/mgt/User/Delete`,
    GetRoleList:`${BASE_URL}/api/mgt/User/GetBindRoleList`,
    SaveUserRole:`${BASE_URL}/api/mgt/User/BindRole`,
  }
}
