import React, {Component} from 'react';
import './Home.css';

export class Home extends Component {
    displayName = Home.name

    constructor(props) {
        super(props);
        this.state = {
            daysSinceSev1: [],
            loading: true
        };

        fetch('api/NewRelic/DaysSinceSev?severity=1')
            .then(response => response.json())
            .then(data => {
                this.setState({daysSinceSev1: data, loading: false});
            });
    }

    static renderDaysSinceCounter(DaysSinceSev) {
        var cardClass = 'card'
        if (DaysSinceSev.daysSinceSev < 4) {
            cardClass += ' severity-critical';
        }
        return (
            <h1 className={cardClass}>
                {DaysSinceSev.daysSinceSev}
            </h1>
        );
    }

    tick() {
        this.setState({
            daysSinceSev1: [],
            loading: true
        });

        fetch('api/NewRelic/DaysSinceSev?severity=1')
        .then(response => response.json())
        .then(data => {
            this.setState({daysSinceSev1: data, loading: false});
        });
    }
    
    componentDidMount() {
        this.interval = setInterval(() => this.tick(), 10 * 60 * 1000); //10 min
    }

    componentWillUnmount() {
        clearInterval(this.interval);
    }

    render() {
        let contents = this.state.loading
            ? <p>
                    <em>Loading...</em>
                </p>
            : Home.renderDaysSinceCounter(this.state.daysSinceSev1);

        return (
            <div>
                <h1>Days Since Sev 1</h1>
                {contents}
            </div>
        );
    }
}
