/* eslint-disable */
import {useState, useEffect, useRef, useCallback, useContext} from "react";
import {useLocation} from 'react-router-dom';
import {Pagination, Tooltip, Button, Image, Upload, Row, Col, message, Input, Popconfirm, Modal, Form} from 'antd';
import {useDispatch, useSelector} from "react-redux";
import PropTypes from 'prop-types';

import {InboxOutlined} from '@ant-design/icons'
import {API_URL} from "app-setting";
import {duong_dan, ReducerKey, actions} from "./Const";
import {ModuleContext} from "./Service";

const {Search, TextArea} = Input;
const {Dragger} = Upload;

const style = {padding: 2};

// Helper function to get parameters from the query string.
function getUrlParam(paramName) {
    var reParam = new RegExp('(?:[\?&]|&)' + paramName + '=([^&]+)', 'i');
    var match = window.location.search.match(reParam);

    return (match && match.length > 1) ? match[1] : null;
}

function useQuery() {
    return new URLSearchParams(useLocation().search);
}

const layout = {
    labelCol: {span: 8},
    wrapperCol: {span: 16},
};
const tailLayout = {
    wrapperCol: {offset: 8, span: 16},
};
const GalleryManager = (props) => {
    const services = useContext(ModuleContext);
    const dispatch = useDispatch()
    const [selectedImage, setSelectedImage] = useState([]);
    const [dataEdit, setDataEdit] = useState({});
    console.log(dataEdit);
    //#region redux------
    //dữ liệu
    const data = useSelector(state => state[ReducerKey].data);
    //Meta
    const {total, page, page_size, sort, search, filter} = useSelector(state => state[ReducerKey].meta);
    //#endregion

    useEffect(() => {
        fetchData({page, page_size, sort, filter, search}, duong_dan);
    }, [page, page_size, sort, filter, search, duong_dan]);

    const fetchData = useCallback((meta, duong_dan) => {
        let newfilter = {...meta.filter};
        meta.filter = newfilter;
        services.getManyFile(meta, duong_dan).subscribe({
            next: (res) => {
                const {meta, data} = res;
                dispatch(actions.setData(data));
                dispatch(actions.setMeta(meta));
            },
            error: (err) => {
                console.log(err);
            }
        });
    }, [services]);
    const setMeta = (meta) => {
        dispatch(actions.setMeta(meta));
    };
    const deleteImg = async (id) => {
        services.del(id).subscribe(() => {
                // beh.alert('Xóa thành công!');
                message.success(`Xoá ảnh thành công!`);
                fetchData({page, page_size, sort, filter, search}, duong_dan);
            },
            async (err) => {
                const errObj = err.error || [];
                message.error(errObj[0].errorMessage || 'Có lỗi xảy ra trong quá trình xóa');
            });
    }


    const handlePageChange = (newPage) => {
        // Implement your page change logic here
        fetchData({page: newPage, page_size, sort, filter, search}, duong_dan);
        console.log("New page:", newPage);
    };

//Phần chỉnh sửa


    const [selectedItems, setSelectedItems] = useState([]);
    const [form] = Form.useForm();
    let query = useQuery();
    const access_token = useSelector((state) => state.oauth.access_token);
    const [visible, setVisible] = useState(false)
    const [loading, setLoading] = useState(false)

    const hide = () => {
        setVisible(false);
    };

    const handleVisibleChange = (item) => {
        setDataEdit(item);
        setVisible(true);
        form.setFieldsValue(item);

    };
    const saveImg = () => {

        setVisible(false);
    };
    const onFinish = async (values) => {
        console.log(...dataEdit);
        const newData = {...dataEdit, ...values}
        console.log(newData);
        services.update(newData, newData.id).subscribe(() => {
                setVisible(false);
                message.success(`Cập nhật thành công!`);
                fetchData({page, page_size, sort, filter, search}, duong_dan);
                form.resetFields();

            },
            async (err) => {
                const errObj = err.error || [];
                message.error(errObj[0].errorMessage || 'Có lỗi xảy ra trong quá trình lưu');
            });

    };

    const onFinishFailed = (errorInfo) => {
        console.log('Failed:', errorInfo);
    };

    const onChosenImage = (item) => {
        if (props.single) {
            if (props.onClickImage) {
                props.onClickImage(item);
            }
        }
        console.log(props.isRichText);
        if (props.isRichText) {
            let tempImage = selectedImage.map(x => `${API_URL}api/file-view/${item.id}`);
            // setSelectedImage([tempImage]);
            tempImage = [API_URL + 'api/file-view/' + item.id];
            // Truyền giá trị trở lại cho cửa sổ cha
            window.opener.postMessage({selectedImages: tempImage}, window.opener.location.href);
            // Đóng cửa sổ con
            window.close();
        }
    }


    const returnFileUrl = (item) => {
        let queryVal = query.get("RichText");
        if (queryVal != null) {
            let funcNum = getUrlParam('RichTextFuncNum');
            let fileUrl = API_URL + `api/file-view/` + item.id;
            window.opener.RichText.tools.callFunction(funcNum, fileUrl, function () {
                let dialog = this.getDialog();
                if (dialog.getName() == 'image') {
                    let element = dialog.getContentElement('info', 'txtAlt');
                    if (element)
                        element.setValue('alt text');
                }
            });
            window.close();
        }

    }

    const hasSelected = (id) => {
        // const index = selectedImage.findIndex(x => x.id === id);
        // return index > -1;
    }

    const onSelectedItem = (item) => {
        if (!props.isRichText) {
            const index = selectedItems.findIndex(x => x.id === item.id);
            console.log(index);
            if (index < 0) {
                let newArray = [...selectedItems, item];
                setSelectedItems(newArray);
                props.selectedItems(newArray);
            } else {
                let newArray = [...selectedItems];
                newArray.splice(index, 1);
                setSelectedItems(newArray);
                props.selectedItems(newArray);
            }
        } else {
            returnFileUrl(item);
        }
    }

    const propsUpload = {
        name: 'FormFile',
        multiple: true,
        action: API_URL + `api/file/tep-tin?duong_dan=${duong_dan}`,
        headers: {
            authorization: 'Bearer ' + access_token,
        },
        beforeUpload: file => {
            if (file.type !== 'image/png' && file.type !== 'image/x-png' && file.type !== 'image/gif' && file.type !== 'image/jpeg') {
                message.error(`${file.name} không đúng định dạng ảnh`);
            }
            return (file.type === 'image/png' || file.type === 'image/x-png' || file.type === 'image/gif' || file.type === 'image/jpeg') ? true : Upload.LIST_IGNORE;
        },
        onChange(info) {
            const {status, response} = info.file;
            if (status !== 'uploading') {
            }
            if (status === 'done') {
                message.success(`${info.file.name} file uploaded successfully.`);
                // let temp = JSON.parse(JSON.stringify(data));
                // console.log(data);
                // console.log(response);
                // console.log(...temp);
                // console.log(response)
                // temp= [...temp, response]
                // setData(...temp, response);
                fetchData({page, page_size, sort, filter, search}, duong_dan);


            } else if (status === 'error') {
                message.error(`${info.file.name} file upload failed.`);
            }
        },
        onDrop(e) {
            console.log('Dropped files', e.dataTransfer.files);
        },
        showUploadList: false
    };
    return (

        <div className="h-100" style={{minHeight: 500}}>
            <Row>
                <Col span={6}>
                    <Row className="m-2">
                        <Search placeholder="Từ khoá..." loading={loading} enterButton onSearch={(keySearch) => {
                            advanceSearch.key_search = keySearch;
                            setAdvanceSearch(advanceSearch);
                            filterPage(advanceSearch);
                        }}/>
                    </Row>
                    <div className="m-2" style={{height: '180px'}}>
                        <Dragger  {...propsUpload}>
                            <p className="ant-upload-drag-icon">
                                <InboxOutlined/>
                            </p>
                            <p className="ant-upload-text">Click or drag file to this area to upload</p>
                            <p className="ant-upload-hint">
                            </p>
                        </Dragger>
                    </div>

                </Col>
                <Col span={18}>

                    <Row>
                        {data.map((item, index) => {
                            return (
                                <Col style={style} key={index} xs={24} sm={12} md={8} lg={8} xl={6}>
                                    <div className="d-flex flex-column border">
                                        <Image height={180} src={`${API_URL}api/file-view/${item.id}`}/>
                                        <div className="p-1 bd-highlight">
                                            <Tooltip title={item.ten}>
                                                <div className="element">
                                                    <div className="small truncate">
                                                        {item.ten}
                                                    </div>

                                                </div>
                                            </Tooltip></div>
                                        <div className="p-1 bd-highlight">
                                            <Button className="m-1" size="small" type="dashed"
                                                    onClick={() => handleVisibleChange(item)}>
                                                Sửa
                                            </Button>
                                            <Popconfirm title="Bạn có chắc chắn xoá không?"
                                                        onConfirm={() => deleteImg(item.id)}>
                                                <Button className="m-1" size="small" type="link" danger>
                                                    Xoá
                                                </Button>
                                            </Popconfirm>
                                        </div>
                                        <div className="p-1 bd-highlight">
                                            <Button className="w-100" type={hasSelected(item.id) ? 'primary' : 'dashed'}
                                                    onClick={() => onChosenImage(item)} size="small">
                                                {hasSelected(item.id) ? 'Bỏ chọn' : 'Chọn'}
                                            </Button>
                                        </div>
                                    </div>
                                </Col>
                            )
                        })}
                    </Row>
                    {total ?
                        <Row>
                            <Pagination showTotal={() => {
                                return (<>Tổng số: {total}</>)
                            }} size="small" total={total}
                                        pageSize={page_size}
                                        onChange={handlePageChange}
                            />
                        </Row> : ''}

                    <Modal title="Thông tin hình ảnh"
                           open={visible}
                           okText="Lưu"
                           cancelText="Đóng lại"
                           onCancel={hide}
                           onOk={() => {
                               form
                                   .validateFields()
                                   .then((values) => {
                                       form.resetFields();
                                       onFinish(values);
                                   })
                                   .catch((info) => {
                                       console.log('Validate Failed:', info);
                                   });
                           }}
                    >
                        <Form
                            form={form}
                            layout="vertical"
                            initialValues={dataEdit}
                            onFinish={onFinish}
                            onFinishFailed={onFinishFailed}
                        >
                            <Form.Item
                                label="Tên ảnh"
                                name="ten"
                                rules={[{required: true, message: 'Trường này không được để trống!'}]}
                            >
                                <Input/>
                            </Form.Item>

                            <Form.Item
                                label="Mô tả"
                            >
                                <TextArea showCount maxLength={255}/>
                            </Form.Item>

                        </Form>
                    </Modal>
                </Col>
            </Row>
        </div>

    )
}
const propTypes = {
    isRichText: PropTypes.bool,
    single: PropTypes.bool,
    selectedItems: PropTypes.func,
    onClickImage: PropTypes.func,
};
const defaultProps = {
    isRichText: true,
    single: false,
    selectedItems: () => {
    }
};
GalleryManager.propTypes = propTypes;
GalleryManager.defaultProps = defaultProps;
export default GalleryManager;