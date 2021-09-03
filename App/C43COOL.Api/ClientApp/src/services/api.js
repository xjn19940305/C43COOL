//跨域代理前缀
// const API_PROXY_PREFIX='/api'
// const BASE_URL = process.env.NODE_ENV === 'production' ? process.env.VUE_APP_API_BASE_URL : ''
const BASE_URL = process.env.VUE_APP_BASE_URL
console.log(process.env)
module.exports = {
  LOGIN: `${BASE_URL}/api/mgt/User/SignInWithPassword`,
  USERINFO:`${BASE_URL}/api/mgt/User/GetUserInfo`,
  ROUTES: `${BASE_URL}/routes`,
  GOODS: `${BASE_URL}/goods`,
  GOODS_COLUMNS: `${BASE_URL}/columns`,
}
