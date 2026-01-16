import {http} from '../utils';

class BaseService {
    constructor(props = {}) {
        this.url = props ? props.url : '';
        this.http = http;
    }

    getMany(props, url) {
        const {page, page_size, sort, filter, search} = props;
        const params = Object.assign({}, {
            page: page,
            page_size: page_size,
            sort: JSON.stringify(sort),
            filter: JSON.stringify(filter),
            search: search
        });
        if (url) {
            return http.get(url, {params: params, logAction: 'SEARCH'});
        }
        return http.get(`${this.url}`, {params: params, logAction: 'SEARCH'});
    }

    getById(id, url) {
        if (url) {
            return http.get(`${url}/${id}`, {logAction: 'ACCESS'});
        }
        return http.get(`${this.url}/${id}`, {logAction: 'ACCESS'});
    }

    create(obj, url) {
        if (url) {
            return http.post(`${url}`, obj, {logAction: 'CREATE'});
        }
        return http.post(`${this.url}`, obj, {logAction: 'CREATE'});
    }

    update(obj, id, url) {
        if (url) {
            return http.put(`${url}/${id}`, obj, {logAction: 'UPDATE'});
        }
        return http.put(`${this.url}/${id}`, obj, {logAction: 'UPDATE'});
    }

    del(id, url) {
        if (url) {
            return http.delete(`${url}/${id}`, {logAction: 'DELETE'});
        }
        return http.delete(`${this.url}/${id}`, {logAction: 'DELETE'});
    }

    deletes(listId = []) {
        return this.http.post(this.url + '/deletes', listId);
    }

    dels(listId = [], id) {
        return http.post(`${this.url}/${id}`, listId);
    }

    downloadDocx(id_file) {
        return this.http.get('api/file/docx?id_file=' + id_file, {responseType: 'blob'});
    }

    downloadPdf(id_file) {
        return this.http.get('api/file/pdf?id_file=' + id_file, {responseType: 'blob'});
    }

    postForm(path, formData) {
        return this.axiosInstance.post(path, formData, {
            headers: {'Content-Type': 'multipart/form-data'},
        });
    }

    getFile(path) {
        return this.axiosInstance.get(path, {
            responseType: 'blob',
        });
    }
}

export {BaseService};
