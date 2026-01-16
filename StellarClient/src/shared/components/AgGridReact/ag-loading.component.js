import React from 'react';
import FadeLoader from 'react-spinners/FadeLoader';
class AgLoading extends React.PureComponent {

    render() {
        return (
            <div className="sweet-loading">
                <FadeLoader height={15} width={5} radius={2} margin={2} color={'#8DE8D6'}  />
            </div>
        );
    }
}
export { AgLoading }; 