import React,{useContext,useState,useEffect} from 'react';
import { Modal,theme,Space
} from 'antd';
import { Icon } from '@iconify/react';
import { BehaviorsContext } from 'shared/services';
const  ConfirmComponent =()=> {
    const context=useContext(BehaviorsContext);
    const [show,setShow]=useState(false);
    const [msg,setMsg]=useState('');
    const {token}=theme.useToken();
    useEffect(() => {
        const subscriptions=context.onShowConfirm.subscribe((res) => {            
            if(!show){
                setMsg(res.msg || '');
                setShow(true);                
            }      
        });
        return ()=>{
            subscriptions?.unsubscribe();
        };
    }, []);
   

   
  
    const handleClose =()=> {
        setShow(false);
        context.onHideConfirm.next(false);
    };
    const handleOK=()=>{
        setShow(false);
        context.onHideConfirm.next(true);
    };
    
    return (
        <Modal
            zIndex={1201}
            title={<Space>
                <span className='anticon'>
                    <Icon width={18} height={18} style={{ color: token.colorWarning }} icon="ant-design:exclamation-circle-filled" />
                </span>                
            Thông báo
            </Space>}
            open={show}
            onOk={handleOK}
            onCancel={handleClose}
        >
                
            <p>{msg}</p>             
        </Modal>
    );
};

export { ConfirmComponent };
