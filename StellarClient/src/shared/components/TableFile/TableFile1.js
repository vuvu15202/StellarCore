import React, { useState, useEffect } from 'react';
import PropTypes from 'prop-types';
import { Form, Input, Table,Row, Col } from 'antd';
import { IUploadFile } from 'shared/components/Input';
const EditableCell = ({
    editing,
    dataIndex,
    title,
    inputType,
    record,
    index,  // eslint-disable-line
    children,
    isRequired,
    handleChange,
    value,
    ...restProps
}) => {
    let inputNode = null;
    switch (inputType) {
    // case 'date':
    //     inputNode = <IDatePicker placeholder="Chọn ngày" format="DD/MM/YYYY" />;
    //     break;
    case 'file':
        inputNode = <IUploadFile multiple onChange={handleChange} duong_dan='quan-ly-khoa-hoc/gui-thong-bao'/>;
        break;
    default:
        inputNode = <Input defaultValue={value} onChange={handleChange} />;
        break;
    }
    return (
        <td {...restProps}>
            {editing ? (
                <Form.Item
                    name={dataIndex}
                    style={{
                        margin: 0,
                    }}
                    rules={[
                        {
                            required: isRequired,
                            message: `Vui lòng nhập ${title}!`,
                        },
                    ]}
                >
                    {inputNode}
                </Form.Item>
            ) : (
                dataIndex == 'can_bo_thuc_hien_id' ? <span>{record?.ten_can_bo_thuc_hien}</span> : inputType == 'date' ? <span></span> : children
            )}
        </td>
    );
};
EditableCell.propTypes = {
    editing: PropTypes.bool,
    dataIndex: PropTypes.string,
    title: PropTypes.any,
    inputType: PropTypes.number | PropTypes.string,
    children: PropTypes.number,
    isRequired: PropTypes.bool,
    record: PropTypes.object,
    handleChange: PropTypes.func,
    value: PropTypes.any
};

export const TableFile= (props) => {
    const { value,
        onChange} = props;
    const [columns, setColumns] = useState([]);
    const [form] = Form.useForm();
    // const { loai_cau_hinh } = useParams();
    const [data, setData] = useState([]);


    useEffect(() => {
        if (Array.isArray(value)) {
            console.log(value);
        } else {
            console.log(value);
        }
    }, [value]);
    useEffect(() => {
        setData([{ten: 'a1', ghi_chu: 'a3' }, {ten: 'b1', ghi_chu: 'b3' }]);
        //getDSCanBo();
        onChange([{id: 1}]);
    }, []);
    useEffect(() => {
        setColumns([
            {
                title: 'STT',
                width: '20px',
                render: (text, item, index) => {
                    let idx = index + 1;
                    return idx;
                }
            },
            {
                key: 'ten',
                title: 'Danh mục tài liệu',
                dataIndex: 'ten'
            },
            {
                key: 'file',
                title: 'Tệp tài liệu',
                dataIndex: 'file',
                editable: true,
            },
            {
                key: 'ghi_chu',
                title: 'Ghi chú',
                dataIndex: 'ghi_chu',
                editable: true,
            }
        ]);
    }, []);
   
    const mergedColumns = columns.map((col) => {
        if (!col.editable) {
            return col;
        }
        return {
            ...col,
            onCell: (record) => ({
                record,
                inputType: getInputType(col),
                dataIndex: col.dataIndex,
                title: col.title,
                editing: true,
                value: col.value,
                isRequired: col.dataIndex == 'file' ? true : false,
                handleChange: handleChange
            }),
        };
    });
    const handleChange = async () => {
        await form.validateFields();
        console.log('-----ssss---', form.getFieldValue());
        const newData = [...data];
        setData(newData);
    };
    const getInputType = (col) => {
        switch (col.dataIndex) {
        case 'file':
            return 'file';
        default:
            'text';
        }
    };
   
    // const getDSCanBo = () => {
    //     const obj = {
    //         page: 1,
    //         page_size: 0,
    //         sort: { 'ten_day_du': 1 },
    //         filter: {},
    //         search: ''
    //     };
    //     services.getManyChuyenGia(obj).subscribe(res => {
    //         setDSCanBo(res.data);
    //     },
    //     (err) => {
    //         console.log(err);
    //     });
    // };
    return (
        <Row className="grid-view-body">
            <Col xs={24} className='grid-table'>
                <Form form={form} component={false}>
                    <Table
                        components={{
                            body: {
                                cell: EditableCell,
                            },
                        }}
                        loading={false}
                        // bordered
                        rowKey="key"
                        dataSource={data}
                        columns={mergedColumns}
                        rowClassName="editable-row"
                        pagination={false}
                    />
                </Form>
            </Col>
        </Row>
    );
};
TableFile.propTypes = {
    value: PropTypes.oneOfType([PropTypes.object, PropTypes.array]),
    onChange: PropTypes.func,
};