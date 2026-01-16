import React, { useState, useEffect } from 'react';
import PropTypes from 'prop-types';
import { Form, theme } from 'antd';
import { IUploadFile } from 'shared/components/Input';
import { http } from 'shared/utils';
import { cloneDeep } from 'lodash';
import './style.scss';
export const TableTaiLieu = (props) => {
    const { onChange, id, ma_form, ds_loai_tai_lieu, duong_dan, disabled, title = 'Tài liệu đính kèm', noTitle = false, isClient = false } = props;
    const [form] = Form.useForm();
    const [data, setData] = useState({});
    const [isLoading, setIsLoading] = useState(false);
    const { token } = theme.useToken();
    
    useEffect(() => {
        if (id != 'new' && id != null) {
            if(isClient){
                form.setFieldsValue({ ds_tai_lieu: ds_loai_tai_lieu });
                setData({ ds_tai_lieu: ds_loai_tai_lieu });
                if (onChange) {
                    onChange([]);
                }
            }else{
                const sub = http.get(`api/qlkh/file/get-all?id=${id}&ma_form=${ma_form}`).subscribe(res => {
                    if (res.length > 0 && ds_loai_tai_lieu != null) {
                        let ds_loai_tai_lieu_new = cloneDeep(ds_loai_tai_lieu);
                        let data = ds_loai_tai_lieu_new.map(x => {
                            x.ds_tep_dinh_kem = res.find(y => y.loai_tai_lieu_id == x.loai_tai_lieu_id)?.ds_tep_dinh_kem;
                            x.ghi_chu = res.find(y => y.loai_tai_lieu_id == x.loai_tai_lieu_id)?.ghi_chu;
                            return x;
                        });
                        form.setFieldsValue({ ds_tai_lieu: data });
                        setData({ ds_tai_lieu: data });
    
                        if (onChange) {
                            onChange(data);
                        }
    
                        setIsLoading(!isLoading);
                    } else {
                        form.setFieldsValue({ ds_tai_lieu: ds_loai_tai_lieu });
                        setData({ ds_tai_lieu: ds_loai_tai_lieu });
                        // if (onChange) {
                        //     onChange([]);
                        // }
                        setIsLoading(!isLoading);
                    }
                }, err => {
                    console.log(err);
                });
                return () => {
                    sub.unsubscribe();
                };
            }
        }
        else {
            if(ds_loai_tai_lieu)
            {
                form.setFieldsValue({ ds_tai_lieu: ds_loai_tai_lieu });
                setData({ ds_tai_lieu: ds_loai_tai_lieu });
                if (onChange) {
                    onChange([]);
                }
            }
       
        }
    }, [id, ds_loai_tai_lieu]);



    const handleChange = (val, allVal) => {
        form.validateFields();
        if (ds_loai_tai_lieu != null) {
            onChange(allVal.ds_tai_lieu);
        }
    };


    return (

        <Form component={false} initialValues={data}
            layout="vertical"
            labelWrap
            requiredMark={(label, { required }) => (<>{label}{required ? <span className='ms-1 text-danger'>*</span> : null}</>)}
            form={form}
            onValuesChange={handleChange}
            scrollToFirstError
        >
            {
                ds_loai_tai_lieu &&
                <Form.Item label={title} noStyle >
                    {!noTitle && <label style={{color:token.colorPrimary}} className='ant-form-item-required-mark-optional mt-2'>{title}</label>}
                    <div className='danh-sach-tai-lieu'>
                        <Form.List name="ds_tai_lieu">
                            {(fields) => (
                                <table >
                                    <tbody>
                                        {/* <tr>
                                                    <td rowSpan={fields.length+2}>{title}</td>
                                                </tr> */}
                                        {fields.map(({ key, name, ...restField }) => (
                                            <tr key={key}>
                                                <td className='loai-tai-lieu'>
                                                    <span>{ds_loai_tai_lieu[key]?.ten_loai_tai_lieu}</span>{ds_loai_tai_lieu[key]?.bat_buoc ? <span className='ms-1 text-danger'>*</span> : null}
                                                </td>
                                                <td className='tep-dinh-kem'>
                                                    <Form.Item
                                                        noStyle
                                                        {...restField}
                                                        name={[name, 'ds_tep_dinh_kem']}
                                                        style={{ marginTop: 13, marginBottom: 13 }}
                                                        rules={[{ required: ds_loai_tai_lieu[key]?.bat_buoc, message: 'File đính kèm không được để trống!' }]}
                                                    >
                                                        <IUploadFile disabled={disabled} multiple duong_dan={duong_dan} />
                                                    </Form.Item>
                                                </td>
                                            </tr>
                                        ))}
                                    </tbody>
                                </table>
                            )}
                        </Form.List>
                    </div>
                </Form.Item>
            }
        </Form>


    );
};
TableTaiLieu.propTypes = {
    value: PropTypes.array,
    onChange: PropTypes.func,
    id: PropTypes.any,
    ds_loai_tai_lieu: PropTypes.array,
    duong_dan: PropTypes.string,
    disabled: PropTypes.bool,
    ma_form: PropTypes.string,
    titleLoaiTaiLieu: PropTypes.string,
};