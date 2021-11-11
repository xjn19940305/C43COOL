import { request, METHOD } from '@/utils/request'
const BASE_URL = `${process.env.VUE_APP_BASE_URL}/api/File`
var FileApi = {
  CreateFile(file) {
    return request(
      `${BASE_URL}/CreateFile?name=${file.name}`,
      METHOD.POST,
      file,
      {
        headers: { 'Content-Type': file.type }
      }
    )
  },
  GetFile(id) {
    return request(
      `${BASE_URL}/GetFile/${id}`,
      METHOD.Get
    )
  }
}
export default FileApi
