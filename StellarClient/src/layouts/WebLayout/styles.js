export const genWarpStyle = (
    prefixCls,
    token
) => ({
    [`.${prefixCls}`]: {
        '.ant-layout-header': {
            backgroundColor: token.colorBgContainer, // Use container background
            borderBottom: `1px solid ${token.colorBorderSecondary}`,
            height: 64, // Default height
            padding: '0 24px',
            display: 'flex',
            alignItems: 'center',
            justifyContent: 'space-between',
        },
        '.app-content': {
             minHeight: 'calc(100vh - 64px)', // Adjust based on header height
        },
        '.ant-layout-sider': {
            borderRight: `1px solid ${token.colorBorderSecondary}`,
        }
    },
});
