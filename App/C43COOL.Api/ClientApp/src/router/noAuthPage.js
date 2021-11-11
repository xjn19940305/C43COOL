// 路由组件注册
const NoAuthRouterMap = [
    {
        ModuleName: "root",
        ModuleType: 0,
        Name: "目视板",
        Path: "dashboard",
        ParentId: "",
        ComponentUrl: "pages/public/dashboard",
    },
    {
        ModuleName: "root",
        ModuleType: 0,
        Name: "文章详情",
        Path: "articleDetail",
        ParentId: "",
        ComponentUrl: "pages/resource/articleDetail"        
    }
]
export default NoAuthRouterMap

