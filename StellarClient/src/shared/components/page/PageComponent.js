import React, { Component } from 'react';
import PropTypes from 'prop-types';
import { Breadcrumb, theme, ConfigProvider } from 'antd';
import { Link } from 'react-router-dom';
import { Icon } from '../Icon';

const PageContext = React.createContext();

const Header = (props) => {
    // eslint-disable-next-line no-unused-vars
    const { title, breadcrumb = [], className, children, ...rests } = props;
    const { token } = theme.useToken();

    return (
        <PageContext.Consumer>
            {() => (
                <section className={['page-header', className].join(' ')} {...rests}>
                    <div className='header-content'>

                        {
                            breadcrumb.length > 0 && <ConfigProvider theme={{
                                components: {
                                    Breadcrumb: {
                                        linkColor: token.colorLink,
                                        linkHoverColor: token.colorLinkHover,
                                    }
                                }
                            }}>
                                <Breadcrumb
                                    items={[{
                                        path: '/',
                                        title: <Icon width={18} height={18} icon="ant-design:home-outlined" />,
                                    }].concat(breadcrumb)}
                                    itemRender={(currentRoute, params, items) => {
                                        const isLast = currentRoute?.path === items[items.length - 1]?.path;
                                        return isLast ? (
                                            <span>{currentRoute.title}</span>
                                        ) : (
                                            <Link to={currentRoute?.path}>{currentRoute.title}</Link>
                                        );
                                    }}
                                />


                            </ConfigProvider>
                        }
                        {title && <h2 className='header-title'>{title}</h2>}
                        
                    </div>
                    {children&&
                     <div className='header-extra'>
                         {children}
                     </div>
                    }
                   
                </section>
            )}
        </PageContext.Consumer>

    );

};
Header.propTypes = {
    className: PropTypes.string,
    title: PropTypes.string,
    breadcrumb: PropTypes.array,
    children: PropTypes.any
};

class Content extends Component {
    static propTypes = {
        className: PropTypes.string
    };
    render() {
        const { className, ...props } = this.props;
        return (
            <PageContext.Consumer>
                {() => (
                    <section className={['page-content', className].join(' ')} {...props}>
                        {props.children}
                    </section>
                )}
            </PageContext.Consumer>

        );
    }

}
class PageComponent extends Component {
    static propTypes = {
        children: PropTypes.any,
        className: PropTypes.any
    };
    render() {
        const { className, ...props } = this.props;
        return (
            <PageContext.Provider {...this.props}
                value={{}}
            >
                <div className={['container-fluid page-warp', className].join(' ')} {...props} >
                    {this.props.children}
                </div>
            </PageContext.Provider>
        );
    }
}
PageComponent.Header = Header;
PageComponent.Content = Content;
export { PageComponent };