import React, { useState, useEffect } from 'react';
import PropTypes from 'prop-types';
import { Row,Form, theme, Divider, Input} from 'antd';
import { http } from 'shared/utils';
import './style.scss';
import { IUploadFile } from 'shared/components/Input';
import { Col } from 'react-bootstrap';
import { cloneDeep } from 'lodash';
export const TableTaiLieuView= (props) => {
    const { id,disabled,ds_loai_tai_lieu} = props;
    const [data, setData] = useState({ds_tai_lieu: ds_loai_tai_lieu});
    const [form] = Form.useForm();
    const {token}=theme.useToken();

    console.log(form);
    useEffect(() => {
        http.get(`api/qlkh/file/get-all-file-by-id?Id=${id}`).subscribe(res => {
            if(res.length > 0 && ds_loai_tai_lieu != null)
            {
                let ds_loai_tai_lieu_new  = cloneDeep(ds_loai_tai_lieu);
                let data = ds_loai_tai_lieu_new.map(x => {
                    x.ds_tep_dinh_kem = res.find(y => y.loai_tai_lieu_id == x.loai_tai_lieu_id)?.ds_tep_dinh_kem;
                    x.ghi_chu = res.find(y => y.loai_tai_lieu_id == x.loai_tai_lieu_id)?.ghi_chu;
                    return x;
                });
                form.setFieldsValue({ds_tai_lieu: data});
                setData({ds_tai_lieu: data}); 
            }else{
                form.setFieldsValue({ds_tai_lieu: ds_loai_tai_lieu});
                setData({ds_tai_lieu: ds_loai_tai_lieu}); 
            }
        }, err => {
            console.log(err);
        });
            
            
    }, [id,ds_loai_tai_lieu]);

   

  

   
    return (
        <Row className="grid-view-body">
            
            <Col span={24} className='grid-table'>
                <Form  component={false}  initialValues={data}
                    layout="vertical"
                    labelWrap
                    requiredMark={(label, { required }) => (<>{label}{required ? <span className='ms-1 text-danger'>*</span> : null}</>)}
                    form={form}
                    scrollToFirstError
                >
                    {
                        ds_loai_tai_lieu != null &&
                            <>
                                <Divider orientation="left" orientationMargin={0} style={{color:token.colorInfo}}>Danh sách tài liệu</Divider>
                                <Form.Item noStyle>
                                    <table style={{width: '100%'}}>
                                        <tr>
                                            <th className='table-cell' style={{width: '3%'}}>STT</th>
                                            <th className='table-cell' style={{width: '20%'}}>Danh mục tài liệu</th>
                                            <th className='table-cell' style={{width: '37%'}}>Tệp tài liệu</th>
                                            <th className='table-cell' style={{width: '40%'}}>Ghi chú</th>
                                        </tr>
                                        <Form.List label="Danh sách tài liệu" name="ds_tai_lieu">                                            
                                            {(fields) => (                                            
                                                <>
                                                    {fields.map(({ key, name, ...restField }) => (
                                                        <tr key={key}>
                                                            <td className='table-cell-td'>
                                                                {key + 1}
                                                            </td>
                                                            <td className='table-cell-td'>
                                                                <span>{ds_loai_tai_lieu[key].ten_loai_tai_lieu}</span>{ds_loai_tai_lieu[key].bat_buoc?<span className='ms-1 text-danger'>*</span>: null}
                                                            </td>
                                                            <td className='table-cell-td'>
                                                                <Form.Item 
                                                                    {...restField}
                                                                    name={[name, 'ds_tep_dinh_kem']}
                                                                >
                                                                    <IUploadFile disabled={disabled} multiple />
                                                                </Form.Item>
                                                            </td>
                                                            <td className='table-cell-td'>
                                                                {
                                                                    disabled ? 
                                                                        <span className="ant-form-text">{data.ds_tai_lieu[key]?.ghi_chu}</span>: 
                                                                        <Form.Item noStyle
                                                                            {...restField}
                                                                            name={[name, 'ghi_chu']}
                                                                        >
                                                                            <Input placeholder='Ghi chú' />
                                                                        </Form.Item>
                                                                }
                                                            </td>       
                                                             
                                                        </tr>
                                                    ))}
                                                </>                                                                                           
                                            )}
                                        </Form.List> 
                                    </table>
                                </Form.Item> 
                            </>
                    } 
                </Form>
            </Col>
            
        </Row>
    );
};
TableTaiLieuView.propTypes = {
    id: PropTypes.any,
    disabled: PropTypes.bool,
    ds_loai_tai_lieu: PropTypes.array,

};