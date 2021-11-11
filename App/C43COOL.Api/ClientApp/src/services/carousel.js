import { request, METHOD } from '@/utils/request'
const BASE_URL = `${process.env.VUE_APP_BASE_URL}/api/mgt/Carousel`
var CarouselApi = {
    Query(params) {
        params = params || {}
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
            `${BASE_URL}/Modify`,
            METHOD.PUT,
            data
        )
    },
    Get(id) {
        return request(
            `${BASE_URL}/Get/?Id=${id}`,
            METHOD.Get
        )
    },
    Delete(params) {
        return request(
            `${BASE_URL}/Delete?${params}`,
            METHOD.DELETE
        )
    }
}
export default CarouselApi
