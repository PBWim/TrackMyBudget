# TrackMyBudget API

**TrackMyBudget** is a simple .NET Core Web API for managing budget entries. It supports basic CRUD operations to create, view, update, and delete budget records.

http://54.151.251.222/swagger

## Features
- **Create**: Add a new budget entry (category, amount, start date, end date).
- **View All**: Retrieve a list of all budgets.
- **View by ID**: Get details of a specific budget by ID.
- **Update**: Modify an existing budget entry.
- **Delete**: Remove a budget entry by its ID.

## Endpoints

1. **GET /api/budgets** - Retrieve all budgets.
2. **GET /api/budgets/{id}** - Get a budget by ID.
3. **POST /api/budgets** - Create a new budget.
4. **PUT /api/budgets/{id}** - Update a budget.
5. **DELETE /api/budgets/{id}** - Delete a budget.

## Swagger API Documentation

You can explore and test the API using Swagger:

- **Swagger UI**: Navigate to `https://localhost:<port>/swagger` to access the API documentation and test the endpoints interactively.

## How to Run

1. Clone the repository:
   ```bash
   git clone https://github.com/yourusername/TrackMyBudget.git
   cd TrackMyBudget

2. Build and run the application:
   ```bash
    dotnet run
   ```

----

## Deploy to AWS Using AWS EC2

* EC2 (Elastic Compute Cloud) provides **virtual machines (called instances)** that you can configure to run applications, websites, databases, or any other software.
* EC2 provides a variety of instance types that are optimized for different workloads :
*    - General Purpose: Balanced CPU, memory, and network resources (e.g., t2.micro).
     - Compute Optimized: High-performance processors (e.g., c5.large).
     - Memory Optimized: For memory-intensive applications (e.g., r5.large).
     - Storage Optimized: High input/output operations for storage (e.g., i3.large).
* You can choose different operating systems for your EC2 instance, such as Amazon Linux, Ubuntu, Windows, and others.
* EC2 instances are secured using Security Groups, which act like virtual firewalls. You can define which ports (e.g., SSH on port 22, HTTP on port 80) are open and which IP addresses are allowed to access the instance.
* **Elastic IP**: You can assign a static IP address (called an Elastic IP) to your instance, which remains the same even if you stop and restart the instance.
* Elasticity: EC2 allows you to increase or decrease capacity depending on your workload. You can scale up during busy periods and scale down when demand is low.
  
> 1. Log in to AWS Management Console.
> 2. Go to the EC2 Dashboard.
> 3. Launch a New Instance. Click on "Launch Instance" and choose an <ins>Amazon Linux</ins> AMI (For .NET Core/6 applications, we can use either Amazon Linux 2 or Windows Server).
> 4. Name your instance: Enter a descriptive name : <ins>Track-My-Budget-Instance</ins>.
> 5. Select <ins>Amazon Linux 2 AMI (HVM), SSD Volume Type – 64-bit (x86)</ins>. This is a lightweight, free-tier eligible option.
> 6. Instance Type : <ins>t2.micro or t3.micro (1 vCPU, 1 GB RAM).</ins>
> 7. Key Pair (Login) : You need a key pair to SSH into the instance (for Amazon Linux) or RDP (for Windows).
>    - 7.1. Create a new key pair.
>    - 7.2. Choose RSA for the key pair type.
>    - 7.3. Download the key pair (it will be a .pem file).
> 8. Network Settings :
>    - 8.1. Auto-assign Public IP : Ensure this is enabled so that your instance gets a public IP address. This allows you to access the instance over the internet.
>    - 8.2. VPC : Choose the default VPC unless you’ve set up custom networking.
>    - 8.3. Security Group : Create a new security group with SSH (port 22), HTTP (port 80) and HTTPS (port 443).
> 9. Configure Storage : The default storage size (8 GB) is usually sufficient for running a .NET console application.

-----

#### Connect to Your EC2 Instance

1. Connect via SSH: Open a terminal and run the following command ```ssh -i /path/to/your-key.pem ec2-user@<your-ec2-public-ip>```
```
ssh -i "C:\Users\PBWim\Documents\Pabodha\Projects\TrackMyBudget\TrackMyBudgetKey.pem" ec2-user@18.141.139.166
```
   - SSH encrypts the data transferred between your local machine and the remote server, ensuring that sensitive information, like passwords or commands, is protected.
   - You use a command like ```ssh``` followed by the IP address or DNS name of the EC2 instance, and authenticate yourself using a private key (for key-based authentication) or a password.
   - AWS generally provides an SSH key pair (.pem file) when you first launch the EC2 instance. You use this key to securely connect.
      - ```-i "your-key.pem"```: Specifies the private key file used to authenticate.
      - ```ec2-user```: The username for the EC2 instance (this could be different depending on the Linux distribution).
      - ```your-instance-public-ip```: The public IP address of your EC2 instance.
   - SSH typically operates over port <ins>22</ins>, so this port needs to be open in the Security Group of your EC2 instance.
  
2. Once connected, install the .NET Core Runtime and ASP.NET Core runtime using the following commands)
```
sudo rpm -Uvh https://packages.microsoft.com/config/centos/7/packages-microsoft-prod.rpm
sudo yum install dotnet-sdk-8.0 -y
```

3. Deploy .NET Core Application. Publish Application on your local machine: Run this from the clone directory in a **new CMD**. ```C:\Users\PBWim\Documents\Pabodha\Projects\TrackMyBudget\TrackMyBudget>```.
```
dotnet publish -c Release -o ./publish
```
   - Since you're likely only using HTTP (port 5000) without an SSL certificate configured, you can safely disable HTTPS redirection in your application. So remember to comment out ```// app.UseHttpsRedirection();``` before this publish. 

4. Transfer the Published Files to the EC2 Instance: In the same CMD, run the ```scp -i /path/to/your-key.pem -r ./publish ec2-user@<your-ec2-public-ip>:/home/ec2-user/TrackMyBudget``` :
   Transfer your app’s published files from your local machine to the EC2 instance.
```
scp -i "C:\Users\PBWim\Documents\Pabodha\Projects\TrackMyBudget\TrackMyBudgetKey.pem" -r ./publish/* ec2-user@54.151.251.222:/home/ec2-user/TrackMyBudget
```
   - ```./publish/*```: This selects all files in the publish directory (the newly built files).
   - ```/home/ec2-user/TrackMyBudget```: This is the destination directory on your EC2 instance where the application files are stored.

5. Navigate to the Directory on the EC2 instance: Use the EC2 Instance Terminal.
```
cd /home/ec2-user/TrackMyBudget
```

6. Run the Application: Start your application using dotnet. If dotnet not installed, install first. Remember to navigate to ```cd /home/ec2-user``` when need to install.
```
sudo rpm -Uvh https://packages.microsoft.com/config/centos/7/packages-microsoft-prod.rpm

sudo yum install dotnet-runtime-8.0

sudo yum install dotnet-sdk-8.0
```

7. Navigate to the Directory on the EC2 instance again and run the application :
```
cd /home/ec2-user/TrackMyBudget
dotnet /home/ec2-user/TrackMyBudget/TrackMyBudget.dll --urls "http://*:5000"
```

8. Step 7: Set Up Nginx as a Reverse Proxy. To make your application publicly accessible on port 80, we'll use Nginx as a reverse proxy.

   - Your .NET Core API is running on port 5000, which is typically not exposed to the public internet directly. Nginx forwards requests from port 80 (standard HTTP) to port 5000. This allows users to access your API using the standard port 80.

   - 8.1. Install Nginx:
       ```
         sudo yum install nginx -y
        ```
   - 8.2. Start and enable Nginx:
       ```
         sudo systemctl start nginx
         sudo systemctl enable nginx
        ```
   - 8.3. Configure Nginx: Open the Nginx configuration file:
       ```
         sudo nano /etc/nginx/nginx.conf
        ```
   - 8.4. Find the ```http``` block and replace the existing server block with the following configuration: Save and exit the file (CTRL+X, then Y, then Enter).
       ```
         server {
          listen 80;
          server_name 54.151.251.222; // This is the Public IP

          location / {
              proxy_pass http://localhost:5000;
              proxy_http_version 1.1;
              proxy_set_header Upgrade $http_upgrade;
              proxy_set_header Connection keep-alive;
              proxy_set_header Host $host;
              proxy_cache_bypass $http_upgrade;
             }
         }
        ```
        
   - 8.5. Restart Nginx to apply the configuration:
       ```
           sudo systemctl restart nginx
        ```

9. Adjust EC2 Security Group. In the AWS Management Console, go to EC2 > Instances > Select your instance > Security Groups. Edit the Inbound Rules to ensure HTTP (port 80) is open to the public.
   - Type: HTTP
   - Port Range: 80
   - Source: 0.0.0.0/0 (or anywhere you want to allow access)

10. Access Your API Publicly
    - ```
         http://54.151.251.222/swagger
      ```


----

## Deploy Code Changes Again

1. Rebuild the Application Locally and transfer the Updated Files to EC2 :
   - ```
      dotnet publish -c Release -o ./publish
      scp -i "C:\Users\PBWim\Documents\Pabodha\Projects\TrackMyBudget\TrackMyBudgetKey.pem" -r ./publish/* ec2-user@54.151.251.222:/home/ec2-user/TrackMyBudget
     ```

2. Restart Your Application on EC2 and Navigate to the project directory.
   - ```
      ssh -i "C:\Users\PBWim\Documents\Pabodha\Projects\TrackMyBudget\TrackMyBudgetKey.pem" ec2-user@54.151.251.222
      cd /home/ec2-user/TrackMyBudget
     ```

3. Stop the currently running application and Then, kill the process:
   - ```
     ps aux | grep dotnet
     kill <PID>
     ```
     
4. Run the updated application: Start your updated application again:
   - ```
     dotnet /home/ec2-user/TrackMyBudget/TrackMyBudget.dll --urls "http://*:5000"
     ```


----

## Set Up the GitHub Actions Workflow (Automate Deployments)

1. Create the GitHub Actions Workflow File - .github/workflows/deploy.yml
2. Check this file to understand the steps : https://github.com/PBWim/TrackMyBudget/blob/main/.github/workflows/deploy.yml
3. Set Up necessary GitHub Secrets :
   - Go to your repository on GitHub. Click on the Settings tab. Scroll down to Secrets and variables > Actions > New repository secret.
   - Add the following secrets: **EC2_PUBLIC_IP** , **EC2_KEY** (Open your .pem file in a text editor, copy the content, and paste it without spaces as the value for this secret.)
   - Since the **EC2_KEY** is stored as a GitHub Secret, it needs to be written to a file before you can use it for SCP. So add a step in the GitHub Actions workflow to create the key file from the secret.
4. Push Code and Trigger the Workflow. And access Your Application.


----

## AWS CloudWatch Logging and Monitoring

* When your application (whether it's running locally, on EC2, or any other environment) needs to interact with AWS services, such as CloudWatch, it requires authentication and authorization to ensure that only authorized applications or users can access those services.
* Your application needs to authenticate itself to AWS, so AWS knows who is trying to access its services. This is typically done using **Access Key ID** and **Secret Access Key** (for IAM Users) or using IAM Roles (when running on EC2, Lambda, etc.).
* Even after authenticating the user or application, AWS needs to verify that the authenticated entity has the right permissions to perform actions such as writing logs to CloudWatch.
     - The IAM User or IAM Role needs to have the CloudWatch permissions (like logs:CreateLogStream, logs:PutLogEvents, etc.) to create log streams and send log data to CloudWatch.
     - Since your application is deployed on an EC2 instance, it's recommended to use an IAM Role attached to your EC2 instance instead of using an IAM User with Access Keys.
     - The IAM Role will automatically provide temporary credentials to your EC2 instance for logging to CloudWatch without needing to manually configure access keys. So if we are using IAM Role, then no need to update anything in the Workflows script as well.
          - Note :: This is only for the EC2. Because from the EC2 Instance Security configs, we can add an IAM Role to the EC2. So we can create an IAM Role with CloudWatchFullAccess and assign it to the EC2. So when your app is running in EC2, you don't want to give any other additional config.
          - But for your local app and for Docker, you need to use an IAM User with  **Access Key ID**, **Secret Access Key** and **Region** to connect to AWS Service from your local machine. So in local ```aws config``` needs to be done.

* Serilog Configuration:
     - In the Program.cs file, we configured Serilog to handle structured logging.
     - We used the AWS CloudWatch Logs sink (send logs to different destinations.  In this case, we configured Serilog to use the AWS CloudWatch Logs sink.) in Serilog to send logs to CloudWatch.
     - The logs are formatted in JSON using the ```CompactJsonFormatter``` for structured logging.
     - The log group used in CloudWatch was defined in the Serilog configuration with the option to automatically create the log group if it doesn’t exist.

* Log Levels:
     - We set up logging levels in the appsettings.json file to control which logs are captured.
     - Unnecessary logs from ASP.NET Core (like Request starting, Request finished) were suppressed by setting LogLevel and Serilog MinimumLevel to Warning for specific namespaces.
 
* By setting up **CloudWatch Alarms**, you can monitor your EC2 instances and application metrics in real-time, respond quickly to issues, and automate actions based on specific conditions. This we can see from the Cloudwatch dashboard as well as a graph.
     - Trigger an alarm when CPU usage exceeds 80% for more than 5 minutes.
     - Metric: CPUUtilization
     - Threshold: Above 80% // refers to the value that a metric must reach or exceed (or drop below) to trigger an alarm
     - Period: 5 minutes
     - Condition: Greater/Equal to 80%
     - Notification: Send an email via SNS.
 

----

## Health Check
http://54.151.251.222/health

1. A dedicated endpoint (usually ```/health```) that reports the health status of the application.
2. This we can enhance more to check the DB connectivity etc as well.


----

## Docker
* One of Docker’s main advantages is that it eliminates the "it works on my machine" problem. By packaging everything (your code, dependencies, configurations) into a container, your application will run the same whether it's on a developer’s local machine, a testing server, or in production. Containers ensure consistency across development, staging, and production environments.
* Once you've packaged your application in a Docker container, you can run it anywhere Docker is installed: on your local machine, on a server, or in the cloud (AWS, Azure, etc.).
* The Docker image you create on your machine is the exact same image that can be deployed on a production server or shared with other developers.
* Without Docker: You have to manually set up and install the correct versions of libraries, runtime environments, databases, and other dependencies on each machine (developer’s machine, testing server, production server).
* With Docker: You package everything your application needs in a Docker image and run it as a container. All environments (development, testing, production) use the exact same image, ensuring the application runs the same way everywhere.

   - Dockerfile: A text file with instructions to build a Docker image. It specifies how to set up the container environment (like what OS, what libraries, etc.).
   - Docker Image: A blueprint for a container. It includes the application code, runtime, and dependencies. Once built, it doesn’t change.
   - Docker Container: A running instance of a Docker image. It behaves like a lightweight, isolated environment for your application.
   - Docker Hub: A public registry where Docker images can be stored and shared. You can push your own images or use images from others.
 
* Steps :
1. Create Dockerfile : A Dockerfile is a simple text file that contains instructions to build a Docker image. It defines the environment where your application will run, such as the base image (e.g., the operating system), dependencies, and commands to run the app.
      - https://github.com/PBWim/TrackMyBudget/blob/main/TrackMyBudget/TrackMyBudget/Dockerfile

2. Build a Docker Image : The image is essentially a snapshot of your application and environment that will be used to create containers.
      ```
         docker build -t trackmybudget-api .
      ```
   * ```docker build``` : This command builds a Docker image based on the Dockerfile.
   * ```-t trackmybudget-api``` : The -t flag allows you to tag your image with a name (trackmybudget-api in this case).
   * ```.``` : The dot (.) represents the current directory where the Dockerfile and application files are located.
  
3. Run a Docker Container : This container will run your application in an isolated environment.
      ```
         docker run --name trackmybudget-container -d -p 8080:80 -e AWS_ACCESS_KEY_ID=<AWS_ACCESS_KEY_ID> -e AWS_SECRET_ACCESS_KEY=<AWS_SECRET_ACCESS_KEY> -e AWS_REGION=<AWS_REGION> trackmybudget-api
      ```
   * ```docker run``` : This command creates and starts a new container.
   * ```-d``` : This flag runs the container in detached mode (in the background).
   * ```-p 8080:80``` : This maps port 80 inside the container to port 8080 on your host machine. So, when you access http://localhost:8080 in your browser, you are accessing the application running inside the container.
   * ```--name trackmybudget-container``` : Assigns a name (trackmybudget-container) to the container, making it easier to manage.
   * ```trackmybudget-api``` : The name of the image to use for the container (the one you built in the previous step).
   * ```-e AWS_ACCESS_KEY_ID=<AWS_ACCESS_KEY_ID> -e AWS_SECRET_ACCESS_KEY=<AWS_SECRET_ACCESS_KEY> -e AWS_REGION=<AWS_REGION>``` : In your local app and for Docker, you need to use an IAM User with  **Access Key ID**, **Secret Access Key** and **Region** to connect to AWS Service from your local machine. So in local execute ```aws config``` and do the needful configurations.
  
4. Push the Docker Image to a Container Registry :  This allows you to store the image in a centralized location and deploy it to other environments like production servers or cloud platforms.
   * Options for Container Registries: **Docker Hub, Amazon ECR (Elastic Container Registry), Azure Container Registry, Google Container Registry**.
  
   - Push Image to Docker Hub: Login to Docker Hub and Tag your image with your Docker Hub username (My username is ```pabodhaw```). And then push the image to Docker Hub.
   - ```
        docker login
        docker tag trackmybudget-api pabodhaw/trackmybudget-api
        docker push pabodhaw/trackmybudget-api
     ```
   - If your Docker image is pushed to Docker Hub (a public or private repository), you can pull it directly from there onto your EC2 instance. If you prefer using ECR (which is AWS’s private container registry), you can push your image to ECR instead of Docker Hub.
   - Once you have pushed the container to the Docker Hub, login to EC2 and install ```Docker```.
   - ```
        ssh -i "C:\Users\PBWim\Documents\Pabodha\Projects\TrackMyBudget\TrackMyBudgetKey.pem" ec2-user@54.151.251.222 // Login to EC2

        // Install Docker in the EC2 instance
        sudo yum update -y
        sudo yum install docker -y
        sudo service docker start // Start the Docker service:
        sudo usermod -a -G docker ec2-user // Add the ec2-user to the Docker group (so you can run Docker commands without sudo):
        sudo reboot // eboot the instance to apply group changes:

        ssh -i "C:\Users\PBWim\Documents\Pabodha\Projects\TrackMyBudget\TrackMyBudgetKey.pem" ec2-user@54.151.251.222 // Log back into the EC2 instance after rebooting:

        docker --version // Verify Docker is installed:
     ```
   - After Docker is installed, You should be able to pull the Docker image from Docker Hub and run it:
     ```
        docker pull pabodhaw/trackmybudget-api:latest    // Pull the Docker image from Docker Hub:
        docker run -d -p 80:80 --name trackmybudget-container pabodhaw/trackmybudget-api:latest // Run the Docker container:
     ```
   - After running the ```run``` command, you'll get this error ```Error starting userland proxy: listen tcp4 0.0.0.0:80: bind: address already in use.```. Because we are already using default port 80 for Nginx redirection when running the app using the dlls. So we need to stop that nginx and run Docker Run once more.
     ```
        docker stop trackmybudget-container // Stop the running container 
        docker rm trackmybudget-container // Remove the Docker container:

        docker run -d -p 80:80 --name trackmybudget-container pabodhaw/trackmybudget-api:latest

        docker logs trackmybudget-container // Check container logs to see if things run correctly
     ```
   - Now check http://54.151.251.222/swagger and make sure things work fine. 
