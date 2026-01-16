import React, { useContext, useRef, useState, useEffect } from 'react';
import { Form, Input, Table } from 'antd';
import PropTypes from 'prop-types';
import { IUploadFile } from 'shared/components/Input';
import { http } from 'shared/utils';
import {cloneDeep} from 'lodash';
const EditableContext = React.createContext(null);
const EditableRow = ({ ...props }) => {
    const [form] = Form.useForm();
    return (
        <Form form={form} component={false}>
            <EditableContext.Provider value={form}>
                <tr {...props} />
            </EditableContext.Provider>
        </Form>
    );
};
const EditableCell = ({
    title,
    editable,
    children,
    dataIndex,
    record,
    handleSave,
    inputType,
    ...restProps
}) => {
    const inputRef = useRef(null);
    const form = useContext(EditableContext);

    const save = async () => {
        try {
            const values = await form.validateFields();
            console.log('values', values);
            //toggleEdit();
            handleSave({
                ...record,
                ...values,
            });
        } catch (errInfo) {
            console.log('Save failed:', errInfo);
        }
    };
   
    // useEffect(() => {
    //     if(record != null)
    //     {
    //         form.setFieldsValue({
    //             [dataIndex]: record[dataIndex],
    //         });
    //     }
    // }, []);    
    let childNode = children;
    let inputNode = null;
    switch (inputType) {
    // case 'date':
    //     inputNode = <IDatePicker placeholder="Chọn ngày" format="DD/MM/YYYY" />;
    //     break;
    case 'file':
        inputNode = <IUploadFile multiple onChange={save}  duong_dan='quan-ly-khoa-hoc/gui-thong-bao'/>;
        break;
    default:
        inputNode = <Input ref={inputRef} onPressEnter={save} onBlur={save} />;
        break;
    }
    if (editable) {
        childNode = 
            <Form.Item
                style={{
                    margin: 0,
                }}
                name={dataIndex}
                rules={[
                    {
                        required: true,
                        message: `${title} is required.`,
                    },
                ]}
            >
                {inputNode}
            </Form.Item>;

    }
    return <td {...restProps}>{childNode}</td>;
};
EditableCell.propTypes = {
    editable: PropTypes.bool,
    dataIndex: PropTypes.string,
    title: PropTypes.any,
    inputType: PropTypes.number | PropTypes.string,
    children: PropTypes.number,
    isRequired: PropTypes.bool,
    record: PropTypes.object,
    handleSave: PropTypes.func,
    value: PropTypes.any,
};
export const TableFile= (props) => {
    const { ds_loai_tai_lieu, onChange, id, buoc_thuc_hien_id} = props;
    const [dataSource, setDataSource] = useState(ds_loai_tai_lieu);
    const defaultColumns = [
        {
            title: 'STT',
            width: '20px',
            render: (text, item, index) => {
                let idx = index + 1;
                return idx;
            }
        },
        {
            title: 'Danh mục tài liệu',
            dataIndex: 'ten',
            width: '30%',
        },
        {
            title: 'Tệp tài liệu',
            dataIndex: 'ds_tep_dinh_kem',
            editable: true
        },
        {
            title: 'Ghi chú',
            dataIndex: 'ghi_chu',
            editable: true
        },
    ];
    useEffect(() => {
        
        if(id != 'new')
        {
            setDataSource([]); 
            http.get(`api/qlkh/file/get-all?id=${id}&buoc_thuc_hien_id=${buoc_thuc_hien_id}`).subscribe(res => {
                if(res.length > 0 && ds_loai_tai_lieu != null)
                {
                    let ds_loai_tai_lieu_new  = cloneDeep(ds_loai_tai_lieu);
                    let data = ds_loai_tai_lieu_new.map(x => {
                        x.ds_tep_dinh_kem = res.find(y => y.loai_tai_lieu_id == x.loai_tai_lieu_id)?.ds_tep_dinh_kem;
                        x.ghi_chu = res.find(y => y.loai_tai_lieu_id == x.loai_tai_lieu_id)?.ghi_chu;
                        return x;
                    });
                    setDataSource(data); 
                }
                
            }, err => {
                console.log(err);
            });
            
        }
    }, [id]);
    const handleSave = (row) => {
        const newData = [...dataSource];
        const index = newData.findIndex((item) => row.key === item.key);
        const item = newData[index];
        newData.splice(index, 1, {
            ...item,
            ...row,
        });
        setDataSource(newData);
        onChange(newData);
        console.log('newdata', newData);
    };
    const components = {
        body: {
            row: EditableRow,
            cell: EditableCell,
        },
    };
    const columns = defaultColumns.map((col) => {
        if (!col.editable) {
            return col;
        }
        return {
            ...col,
            onCell: (record) => ({
                record,
                editable: col.editable,
                dataIndex: col.dataIndex,
                title: col.title,
                inputType: getInputType(col),
                handleSave,
            }),
        };
    });
    const getInputType = (col) => {
        switch (col.dataIndex) {
        case 'ds_tep_dinh_kem':
            return 'file';
        default:
            'text';
        }
    };
    return (
        <div>
            <Table
                components={components}
                rowClassName={() => 'editable-row'}
                bordered
                dataSource={dataSource}
                columns={columns}
            />
        </div>
    );
};
TableFile.propTypes = {
    value: PropTypes.oneOfType([PropTypes.object, PropTypes.array]),
    onChange: PropTypes.func,
    id: PropTypes.any,
    buoc_thuc_hien_id:PropTypes.any,
    ds_loai_tai_lieu: PropTypes.array
};