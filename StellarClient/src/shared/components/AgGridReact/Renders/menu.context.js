import React from 'react';
import PropTypes from 'prop-types';
import '../style.scss';
import { Overlay, Popover, Dropdown, Row, Col } from 'react-bootstrap';
import { agService } from '../service';

class MenuContext extends React.PureComponent {
    static propTypes = {
        child: PropTypes.any,
        children: PropTypes.any,
        menus: PropTypes.array,
    };
    constructor(props) {
        super(props);
        this.state= {
            show: false
        };
        this.onHide= this.onHide.bind(this);
        this.onOutSide= this.onOutSide.bind(this);
        
    }
    componentDidMount() {
        document.addEventListener('contextmenu', this.onOutSide);
        agService.sendToRender.subscribe((res) => {
            this.setState({ editMode: res });
        });
    }
    componentWillUnmount() {
        document.removeEventListener('contextmenu', this.onOutSide);
    }
    onHide() {
        this.setState({ show:false });
    }
    onOutSide(e) {
        const wasOutside = !(this.target.contains(e.target));
        if (wasOutside) this.setState({ show: false, });
    }
    contextMenu(e) {
        e.preventDefault();
        this.state.editMode && this.setState({ show: true });
    }

    render() {
        const { child } = this.props;
        let menus = this.props.menus || [];
        return (
            <div onContextMenu={this.contextMenu.bind(this)} ref={(e) => this.target = e} className='context'
                style={{
                    width: this.props.child.props.eGridCell.clientWidth,
                    height: this.props.child.props.eGridCell.clientHeight,
                    marginLeft: -17
                }}
            >
                {child}
                <Overlay className="over-lay" target={this.target} show={this.state.show}
                    placement="right"
                    onHide={() => this.onHide()}
                    rootClose={true}
                    rootCloseEvent='mousedown'>
                    {({  
                        // eslint-disable-next-line no-unused-vars
                        arrowProps, 
                        show, ...props }) => (
                        <div { ...props }>
                            {
                                menus.length > 0 &&
                                <Popover show={show} id="popover-basic" ref={(e) => this.menu = e}>
                                    <Popover.Content>
                                        {
                                            menus.map((x, i) => {
                                                return (
                                                    <Row key={i} onClick={() => this.setState({ show: !this.state.show })}>
                                                        <Col>
                                                            <Dropdown.Item onClick={x.callback}>{x.title}</Dropdown.Item>
                                                            <Dropdown.Divider></Dropdown.Divider>
                                                        </Col>
                                                    </Row>
                                                );
                                            })
                                        }
                                        
                                    </Popover.Content>
                                </Popover>
                            }
                        </div>
                    )}
                </Overlay>
            </div>
        );
    }
}
export { MenuContext };