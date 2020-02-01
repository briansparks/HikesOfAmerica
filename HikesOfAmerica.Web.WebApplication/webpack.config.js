module.exports = {
        entry: './index.js',  
        output: {    
          path: __dirname + '/dist',    
          publicPath: '/',    
          filename: 'bundle.js'  
        },  
        devServer: {    
            contentBase: '.', 
            hot: true
        },  
        module: {    
            rules: [{      
              test: /\.(js|jsx)$/,      
              exclude: /node_modules/,      
              use: ['babel-loader']    
            },
            {
              test: /\.css$/,
              use: [
                  { loader: "style-loader" },
                  { loader: "css-loader" }
              ]
          },
          {
              test: /\.svg$/,
              loader: 'svg-inline-loader'
          }] 
        },
        resolve: {
            extensions: ['*', '.js', '.jsx']
          },
    };
