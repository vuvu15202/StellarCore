import { createContext } from 'react';
import { BaseService } from 'shared/services';
import { endPoint } from './Const';
import { Subject } from 'rxjs';
class ModuleService extends BaseService {
    constructor(props) {
        const _props = Object.assign({}, { url: endPoint }, props);
        super(_props);
        this.sendToForm = new Subject();
    }
    getManyFile(props,duong_dan) {
        const { page, page_size, sort, filter, search } = props;
        const params = Object.assign({}, {
            page: page,
            page_size: page_size,
            sort: JSON.stringify(sort),
            filter: JSON.stringify(filter),
            search: search
        });
        return this.http.get(`api/file/tep-tin/get-file-by-duong-dan/${duong_dan}`, { params: params });
    }
    uploadFile(obj,duong_dan){
        console.log(obj);
        return this.http.post(`api/file/tep-tin?duong_dan=${duong_dan}`,obj);

    }
}
const service = new ModuleService();
const ModuleContext = createContext(service);
export { ModuleContext, ModuleService };