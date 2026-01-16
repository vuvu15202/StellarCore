import React from 'react';
import PropTypes from 'prop-types';
import { Document, Page } from 'react-pdf/dist/esm/entry.webpack';
import { Spin } from 'antd';
import { SizeMe } from 'react-sizeme';
class PdfPreviewComponent extends React.PureComponent {
    static propTypes = {
        onLoadSuccess: PropTypes.func,
        file: PropTypes.any,
        is_loading: PropTypes.any
    };
    constructor(props) {
        super(props);
        this.state = {
            numPages: null,
            scale: 1.0,
            loading: true
        };
        this.onDocumentLoadSuccess = this.onDocumentLoadSuccess.bind(this);
    }
    onDocumentLoadSuccess({ numPages }) {
        this.setState({ numPages: numPages });
    }
    render() {
        const {
            // eslint-disable-next-line no-unused-vars
            onLoadSuccess,
            ...props
        } = this.props;
        return (
            <div className="document-warp">
                <Spin spinning={this.props.is_loading}>
                    <SizeMe>{({ size }) => {
                        console.log(size);
                        return (<Document
                            noData="Không tìm thấy tệp"
                            onLoadSuccess={this.onDocumentLoadSuccess.bind(this)}
                            renderMode="svg"
                            {...props}
                        >
                            {Array.apply(null, Array(this.state.numPages))
                                .map((x, i) => i + 1)
                                .map((page, j) => <Page
                                    renderTextLayer={false}
                                    // loading={<LoadingComponent loading={true}/>}
                                    key={j}
                                    pageNumber={page}
                                    width={size.width}
                                />)}

                        </Document>);
                    }}</SizeMe>
                </Spin>

            </div>

        );
    }

}

export { PdfPreviewComponent };