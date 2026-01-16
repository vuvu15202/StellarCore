import React, { useState, useEffect, useContext } from 'react';
import PropTypes from 'prop-types';
import {
    Upload, Button, Space,
    Image, theme, Spin, message
} from 'antd';
import { useStyleRegister } from '@ant-design/cssinjs';
import classNames from 'classnames';
import { http } from 'shared/utils';
import { Icon } from '@iconify/react';
import avatarDefault from 'assets/images/avatar.png';
import { FILE_URL, FILE_MAX_SIZE, FILE_ACCEPT_LIST } from '../../app-setting';
import { BehaviorsContext } from 'shared/services/BehaviorsService';
import mime from 'mime';



const genUploadWarpStyle = (
    prefixCls,
    token,
) => ({
    [`.${prefixCls}`]: {
        '.ant-image': {
            'img': {
                objectFit: 'cover'
            }

        },
        '.ant-upload': {
            position: 'relative',
            '.i-upload-mask': {
                position: 'absolute',
                top: 0,
                bottom: 0,
                height: '100%',
                width: 200,
                margin: 'auto',
                color: token.colorTextLightSolid,
                transition: 'opacity 0.3s',
                background: token.colorBgMask,
                display: 'flex',
                justifyContent: 'center',
                cursor: 'pointer',
                opacity: 0,

                '&:hover': {
                    opacity: 1,
                }

            },
        }


    },
});


export const IUploadFile = (props) => {
    const { value,
        onChange,
        duong_dan = '',
        multiple,
        avatar = false,
        accept = FILE_ACCEPT_LIST.join(','),
        disabled,
        hiddenText,
        onFinished,
        confirmDelete = true,
        ...restProps } = props;
    const [fileList, setFileList] = useState([]);
    const [acceptVal, setAcceptVal] = useState([]);
    const beh = useContext(BehaviorsContext);
    useEffect(() => {
        if (Array.isArray(value)) {
            setFileList(value.map(x => {
                return {
                    ...x,
                    url: x.id ? `${FILE_URL}${x.id}` : null
                };
            }));
        } else {
            if (value) {
                value.url = value.id ? `${FILE_URL}${value.id}` : null;
                setFileList([value]);
            } else {
                setFileList([]);
            }
        }
    }, [value]);
    useEffect(() => {
        if (avatar) {
            setAcceptVal('image/*');
        } else {
            setAcceptVal(accept);
        }

    }, [accept, avatar]);

    const customRequest = (request) => {
        let formdata = new FormData();
        formdata.append('FormFile', request.file);
        http.post(`api/file/tep-tin?duong_dan=${duong_dan}`,
            formdata,
            {
                reportProgress: true,
                observe: 'events',
                onUploadProgress: function (ev) {
                    var percent = Math.round((ev.loaded * 100) / ev.total);
                    request.onProgress({ percent });
                }
            }).subscribe(res => {
                request.onSuccess(res[0]);
                if (onFinished) {
                    onFinished(res[0]);
                }
            }, err => {
                request.onError(err);
            });

    };
    const handleChange = (info) => {
        const newFileList = info.fileList.map(file => {
            if (file.response) {
                // Component will show file.url as link
                file = { ...file, ...file.response };
            }
            if (file.id) {
                file.url = `${FILE_URL}${file.id}`;
            }
            return file;
        });
        setFileList(newFileList);
        if (onChange) {
            if (multiple) {
                onChange(newFileList);
            } else {
                onChange(newFileList.slice(-1)[0] || null);
            }
        }
    };

    const handleRemove = async (item) => {
        if (confirmDelete) {
            if (!await beh.confirm('Bạn có chắc xóa tệp này'))
                return false;
        }
        if (item.id) {
            setFileList((files) => {
                return files.map(x => {
                    if (x.id === item.id) {
                        x.status = 'uploading';
                        x.percent = 0;
                    }

                    return x;
                });
            });
            return new Promise((resolve, reject) => {
                http.delete(`api/file/tep-tin/${item.id}`, { reportProgress: true, observe: 'events' })
                    .subscribe(() => {
                        console.log('xóa file thành công');
                        resolve(item);
                    }, err => {
                        console.log(err);
                        reject(err);
                    });
            });
        } else {
            return item;
        }
    };
    const handleBeforeUpload = (file) => {
        let isAccepted = true;
        if (acceptVal) {
            const acceptList = acceptVal.split(',');
            console.log(mime.getType(file.name));
            isAccepted = acceptList.some(x => {
                // eslint-disable-next-line no-useless-escape                
                return (new RegExp(x.replace('*', '.\*'))).test(mime.getType(file.name));
            });

        }
        if (!isAccepted) {
            message.error(`Tệp ${file.name} không đúng định dạng. \nChức năng chỉ cho phép các định dạng ( ${acceptVal} )!`, 5);
        }

        const isLtM = file.size / 1024 / 1024 < FILE_MAX_SIZE;
        if (!isLtM) {
            message.error(`Dung lượng tệp ${file.name} không được vượt quá ${FILE_MAX_SIZE}MB!`, 5);
        }
        return (isAccepted && isLtM) || Upload.LIST_IGNORE;
    };

    const prefixCls = 'i-upload';
    const infoStyle = { ...theme.useToken(), ...{ path: [prefixCls] } };
    const wrapSSR = useStyleRegister(infoStyle, () => [genUploadWarpStyle(prefixCls, infoStyle.token)]);
    return wrapSSR(
        <>
            {avatar ?
                <Upload {...restProps}
                    fileList={fileList}
                    customRequest={customRequest}
                    onChange={handleChange}
                    onRemove={handleRemove}
                    beforeUpload={handleBeforeUpload}
                    accept={acceptVal}
                    maxCount={1}
                    showUploadList={false}
                    className={classNames(prefixCls, infoStyle.hashId)}
                    disabled={disabled}
                >
                    <Spin spinning={fileList[0]?.status === 'uploading'}>
                        <Image width={200} height={267} src={fileList[0]?.url ? fileList[0]?.url : avatarDefault} preview={false} fallback={avatarDefault} />
                    </Spin>
                    {!disabled && <Space className='i-upload-mask'>
                        <Icon width={18} icon="ant-design:upload-outlined" />
                        Thay ảnh
                    </Space>
                    }
                </Upload>
                :
                <Upload {...restProps}
                    fileList={fileList}
                    customRequest={customRequest}
                    multiple={multiple}
                    beforeUpload={handleBeforeUpload}
                    onChange={handleChange}
                    onRemove={handleRemove}
                    accept={acceptVal}
                    disabled={disabled}
                >
                    {!disabled && <Button type="dashed">
                        <Space>
                            <Icon icon="ant-design:upload-outlined" />
                            {!hiddenText && 'Chọn tệp'}
                        </Space>
                    </Button>}
                </Upload>
            }
        </>

    );
};

IUploadFile.propTypes = {
    value: PropTypes.oneOfType([PropTypes.object, PropTypes.array]),
    onChange: PropTypes.func,
    duong_dan: PropTypes.string,
    multiple: PropTypes.bool,
    avatar: PropTypes.bool,
    editMode: PropTypes.bool,
    accept: PropTypes.string,
    disabled: PropTypes.bool,
    hiddenText: PropTypes.bool,
    confirmDelete: PropTypes.bool
};



