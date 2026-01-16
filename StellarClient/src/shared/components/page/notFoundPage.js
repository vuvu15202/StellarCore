import React from 'react';
import { Result, Button } from 'antd';
import { useNavigate} from 'react-router-dom';
export const NotFoundPage= () => {
    const navigate=useNavigate();
    const goBack=()=>{
        navigate(-1);
    };
    return (
        <Result
            style={{marginTop:'20vh'}}
            status="404"
            title="404"
            subTitle="Có lỗi xảy ra, trang bạn truy cập không tồn tại."
            extra={<Button onClick={goBack} type="primary">Quay lại</Button>}
        />
    );
};
